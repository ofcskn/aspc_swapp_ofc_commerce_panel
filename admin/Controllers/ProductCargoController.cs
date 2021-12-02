using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using Service.Abstract;

namespace admin.Controllers
{
    [Authorize]
    public class ProductCargoController : Controller
    {
        private readonly IUnitOfWork _uow;
        public ProductCargoController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [Authorize(Policy = "Staff")]
        [HttpPost]
        public JsonResult PublishStatus(bool enabled, int itemId)
        {
            var item = _uow.ProductCargo.GetById(itemId);
            if (item != null)
            {
                item.Enabled = enabled;
                _uow.ProductCargo.Update(item);
                if (enabled == true)
                {
                    return Json("start");
                }
                return Json("pause");
            }
            return Json("no");
        }
        [Authorize(Policy = "Current")]
        [HttpGet]
        public IActionResult Current(string q)
        {
            if (q != null)
            {
                return View("List", _uow.ProductCargo.SearchByCargoCode(q).Where(p => p.CurrentId == _uow.Admin.GetIdByAdmin));
            }
            else
            {
                return View("List", _uow.ProductCargo.GetAllPC().Where(p => p.CurrentId == _uow.Admin.GetIdByAdmin));
            }
        }
        [Authorize(Policy = "Current")]
        [HttpGet]
        public IActionResult List(string filter, string q, int cid)
        {
            int currentId = _uow.Admin.GetIdByAdmin;
            if (_uow.Admin.GetRoleByAdmin != "current")
            {
                currentId = cid;
            }
            var completedList = _uow.ProductCargo.GetAllPCByEnabled(true);
            var inProcessList = _uow.ProductCargo.GetAllPCByEnabled(false);
            var bySearchList = _uow.ProductCargo.SearchByCargoCode(q);

            if (_uow.Admin.GetRoleByAdmin == "current" || cid != 0)
            {
                if (filter == "completed")
                {
                    return View(completedList.Where(p => p.CurrentId == currentId).Include(p => p.CargoCompany).Include(p => p.Current));
                }
                else if (filter == "in-process")
                {
                    return View(inProcessList.Where(p => p.CurrentId == currentId).Include(p => p.CargoCompany).Include(p => p.Current));
                }
                else if (q != null)
                {
                    return View(bySearchList.Where(p => p.CurrentId == currentId).Include(p => p.CargoCompany).Include(p => p.Current));
                }
                else
                {
                    return View(_uow.ProductCargo.GetAll().Where(p => p.CurrentId == currentId).Include(p => p.CargoCompany).Include(p => p.Current));
                }
            }
            else
            {
                if (filter == "completed")
                {
                    return View(_uow.ProductCargo.GetAllPCByEnabled(true));
                }
                else if (filter == "in-process")
                {
                    return View(inProcessList);
                }
                else if (q != null)
                {
                    return View(bySearchList);
                }
                else
                {
                    return View(_uow.ProductCargo.GetAllPC());
                }
            }

        }
        [HttpGet]
        public IActionResult Manage(int? id)
        {
            if (id != null)
            {
                return View(_uow.ProductCargo.GetById(Convert.ToInt32(id)));
            }
            return View(new ProductCargo());
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
        public IActionResult Manage(ProductCargo entity, string currentCode, string productCode)
        {
            var current = _uow.Current.GetByNo(currentCode);
            var product = _uow.Product.GetProductByBarcode(productCode);

            if (entity.Id != 0)
            {
                var pcargo = _uow.ProductCargo.GetById(entity.Id);
                pcargo.Title = entity.Title;
                pcargo.Description = entity.Description;
                pcargo.EndDate = entity.EndDate;
                _uow.ProductCargo.Update(entity);
                return RedirectToAction("List");
            }
            else
            {
                if (current != null && product != null)
                {
                    //Create Cargo Code
                    Random randomNumber = new Random();
                    String serialRandom = "P-" + randomNumber.Next(0, 1000000).ToString("D6");
                    var item__list = _uow.ProductCargo.GetAll().Where(p => p.CargoNo == serialRandom).ToList();
                    if (item__list.Count() != 0)
                    {
                        serialRandom = "P-" + randomNumber.Next(0, 1000000).ToString("D6");
                    }
                    entity.CargoNo = serialRandom;
                    var cargoCompany = _uow.CargoCompany.GetById(entity.CargoCompanyId);
                    entity.CurrentId = current.Id;
                    entity.CargoChaseLink = cargoCompany.WebSite + "/cargo-chase-example/" + "SWAPP-OFC-" + serialRandom;

                    QRCodeGenerator _qrCode = new QRCodeGenerator();
                    QRCodeData _qrCodeData = _qrCode.CreateQrCode(entity.CargoChaseLink, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(_qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    entity.QrCode = "data:image/png;base64," + Convert.ToBase64String(BitmapToBytesCode(qrCodeImage));

                    entity.ProductId = product.Id;
                    entity.StartDate = DateTime.Now;
                    _uow.ProductCargo.Add(entity);

                    //Current Notification To Admins
                    var nottype = _uow.NotificationType.GetNTByType("product-cargo-for-current");
                    string notificationTitle = entity.CargoNo + " " + nottype.Message;
                    try
                    {
                        //Add Notification
                        Notification notification = new Notification
                        {
                            Enabled = false,
                            NotTypeId = nottype.Id,
                            SendDate = DateTime.Now,
                            Title = notificationTitle,
                            UserId = entity.CurrentId,
                            UserRole = "current"
                        };
                        _uow.Notification.Add(notification);
                    }
                    catch (Exception)
                    {
                    }
                    return RedirectToAction("List");
                }
                else
                {
                    if (current == null)
                    {
                        ViewBag.ErrorMessage = "Faturalandırma için sistemimizde kayıtlı bir müşteri numarasının olması gerekmektedir.";
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Kargoya verilecek bir ürün seçmeniz gerekmektedir.";
                    }
                    return View(entity);
                }
            }

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _uow.ProductCargo.GetById(Convert.ToInt32(id));
            if (item != null)
            {
                _uow.ProductCargo.Delete(item);
                return RedirectToAction("List");
            }
            return View(item);
        }
    }
}
