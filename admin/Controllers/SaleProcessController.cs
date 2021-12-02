using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Context;
using Entity.Models;
using Entity.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service.Abstract;
using Service.Utilities;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using admin.Models;
using QRCoder;

namespace admin.Controllers
{
    [Authorize]
    public class SaleProcessController : Controller
    {
        private readonly IUnitOfWork _uow;
        public SaleProcessController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [Authorize(Policy = "Staff")]
        [HttpPost]
        public JsonResult PublishStatus(bool enabled, int saleProcessId)
        {
            var item = _uow.SaleProcess.GetById(saleProcessId);
            if (item != null)
            {
                item.Status = enabled;
                _uow.SaleProcess.Update(item);
                if (enabled == true)
                {
                    return Json("start");
                }
                return Json("pause");
            }
            return Json("no");
        }
        [HttpGet]
        [Authorize(Policy = "Current")]
        public async Task<IActionResult> Current(int pageNumber = 1)
        {
            var list = _uow.SaleProcess.GetAllByDate().Where(p=>p.CurrentId == _uow.Admin.GetIdByAdmin);//Your list
            return View("List",await PaginatedList<SaleProcess>.CreateAsync(list, pageNumber, 10));

        }
        [Authorize(Policy = "Staff")]
        [HttpGet]
        public async Task<IActionResult> List(int? staffId, int pageNumber = 1)
        {
            if (staffId != null)
            {
                var list = _uow.SaleProcess.GetAllByStaff(Convert.ToInt32(staffId));//Your list
                return View(await PaginatedList<SaleProcess>.CreateAsync(list, pageNumber, 10));
            }
            else
            {
                var list = _uow.SaleProcess.GetAllByDate();//Your list
                return View(await PaginatedList<SaleProcess>.CreateAsync(list, pageNumber, 10));
            }
        }
        [Authorize(Policy = "Staff")]
        [HttpGet]
        public IActionResult Manage(int? id, string filter)
        {
            if (id != null)
            {
                ViewBag.Membership = true;
                return View(_uow.SaleProcess.GetById(Convert.ToInt32(id)));
            }
            else
            {
                if (filter == "member")
                {
                    ViewBag.Membership = true;
                    return View(new SaleProcess());
                }
                else
                {
                    ViewBag.Membership = false;
                    return View(new SaleProcess());
                }
            }
        }
        [NonAction]
        private static Byte[] BitmapToBytesCode(Bitmap image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult Manage(SaleProcess entity, string MemberNo, string ProductBarcode)
        {
            if (entity.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    Random randomNumber = new Random();
                    String serialRandom = randomNumber.Next(0, 1000000).ToString("D6");
                    var invoice_list = _uow.Invoice.GetAll().Where(p => p.SerialNumber == serialRandom).ToList();
                    if (invoice_list.Count() != 0)
                    {
                        serialRandom = randomNumber.Next(0, 1000000).ToString("D6");
                    }
                    if (entity.ProductId == null || entity.ProductId == 0)
                    {
                        entity.ProductId = _uow.Product.GetIdByBarcode(ProductBarcode);
                    }

                    //Qr Coder
                    if (ProductBarcode == null)
                    {
                        ProductBarcode = _uow.Product.GetById(entity.ProductId).Barcode;
                    }

                    QRCodeGenerator _qrCode = new QRCodeGenerator();
                    QRCodeData _qrCodeData = _qrCode.CreateQrCode(ProductBarcode, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(_qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    ViewBag.qrCodeImage = "data:image/png;base64," + Convert.ToBase64String(BitmapToBytesCode(qrCodeImage));

                    int productId = Convert.ToInt32(entity.ProductId);
                    int staffId = Convert.ToInt32(_uow.Invoice.GetIdByStaff);
                    if (MemberNo != null)
                    {
                        entity.CurrentId = _uow.Current.GetByNo(MemberNo).Id;
                        Invoice invoice = new Invoice
                        {
                            StaffId = staffId,
                            CurrentId = Convert.ToInt32(entity.CurrentId),
                            SendDate = DateTime.Now,
                            TaxAdministration = "Ofcskn",
                            SerialNumber = serialRandom,
                        };

                        _uow.Invoice.Add(invoice);

                        var nottype = _uow.NotificationType.GetNTByType("send-invoice");
                        try
                        {
                            string notificationTitle = _uow.Admin.GetUserNameSurname("staff", Convert.ToInt32(entity.StaffId), "name") + " " + nottype.Message;
                            //Add Notification
                            Notification notification = new Notification
                            {
                                Enabled = false,
                                NotTypeId = nottype.Id,
                                SendDate = DateTime.Now,
                                Title = notificationTitle,
                                UserId = Convert.ToInt32(entity.CurrentId),
                                UserRole = "current"
                            };
                            _uow.Notification.Add(notification);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        entity.InvoiceId = invoice.Id;
                    }
                    _uow.SaleProcess.UpdateProductStock(entity, productId, staffId);

                    return RedirectToAction("List");
                }
                return View(entity);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _uow.SaleProcess.Update(entity);
                    return RedirectToAction("List");
                }
                return View(entity);
            }
        }
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(int id)
        {
            SaleProcess sale = _uow.SaleProcess.GetById(id);
            _uow.SaleProcess.Delete(sale);
            return Json("ok");
        }

        [Authorize(Policy = "Staff")]
        public IActionResult GetTotal(int productId, int amount, string barcode)
        {
            var data = "";
            var product = _uow.Product.GetById(productId);
            if (barcode != null)
            {
                product = _uow.Product.GetProductByBarcode(barcode);
            }
            if (product != null)
            {
                var salePrice = product.SalePrice;
                var total = salePrice * amount;
                var data2 = new { total = total, salePrice = salePrice };
                return Json(data2);
            }
            return Json(data);
        }
    }
}
