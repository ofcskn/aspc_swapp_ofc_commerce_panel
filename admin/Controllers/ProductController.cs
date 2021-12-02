using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Context;
using Entity.Models;
using Entity.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace admin.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _uow;
        public ProductController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public IActionResult CompleteOrder()
        {
            var products = _uow.Cookie.GetCookie("product-list");
            for (int i = 0; i < products.Length; i++)
            {
                SaleProcess saleProcess = new SaleProcess
                {
                    Amount = "1500"
                };
                _uow.SaleProcess.Add(saleProcess);

                //Sale Process Notification To Everyone
                var nottype = _uow.NotificationType.GetNTByType("new-sale-process-for-staff");
                string notificationTitle = _uow.Admin.GetUserNameSurname("staff", Convert.ToInt32(saleProcess.StaffId), "name") + " " + nottype.Message;
                _uow.Notification.AddListForAdmin(nottype.Id, notificationTitle, _uow.Admin.GetAllByEnabled(true));
            }
            return View();
        }
        public IActionResult ControlBarcode(string barcode)
        {
            if (_uow.Product.ControlBarcode(barcode) == true)
            {
                return Json("true");
            }
            else
            {
                return Json("false");
            }
        }
        [Authorize(Policy = "Staff")]
        public IActionResult Basket()
        {
            var products_cookie = _uow.Cookie.GetCookie("product-items");
            if (products_cookie != null)
            {
                var splitted = products_cookie.Split("||");
                List<Product> product_list = new List<Product>();
                foreach (var item in splitted)
                {
                    if (item != "")
                    {
                        var product_id = item.Split("-")[0];
                        product_list.Add(_uow.Product.GetProductById(Convert.ToInt32(product_id)));
                    }
                }
                return View(product_list);
            }
            else
            {
                ViewBag.EmptyBasket = "Sepetinizde henüz ürün yoktur.";
            }
            return View();
        }
        [Authorize(Policy = "Staff")]
        public JsonResult ShoppingBasket(string filter, int productId)
        {
            if (filter == "add")
            {
                string productCount = _uow.Cookie.GetCookie("basket-item-count");
                int pcount = Convert.ToInt32(productCount);
                pcount++;

                string products = _uow.Cookie.GetCookie("product-items");
                if (products == "" || products == null)
                {
                    products = "||" + productId + "-1";
                }
                else
                {
                    var splitted = products.Split("||");
                    foreach (var item in splitted)
                    {
                        if (item != "")
                        {
                            var product_id = item.Split("-")[0];
                            var item_count = Convert.ToInt32(item.Split("-")[1]);
                            item_count++;
                            if (Convert.ToInt32(product_id) == productId)
                            {
                                var last = Convert.ToInt32(item.Substring(item.Length - 1));
                                last++;
                                products = products.Substring(0, products.Length - 1) + last;
                            }
                            else
                            {
                                if (products.Contains("||" + productId))
                                {

                                }
                                else
                                {
                                    products = products + "||" + productId + "-1";
                                }
                            }
                        }
                    }
                }


                //if (products == "" || products == null || products.Contains("||" + productId))
                //{
                //    products = "||" + productId + "-" + pcount;
                //}
                //else
                //{
                //    var splitted = products.Split("||");
                //    foreach (var item in splitted)
                //    {
                //        if (item != "")
                //        {
                //            var product_id = item.Split("-")[0];
                //            var item_count = Convert.ToInt32(item.Split("-")[1]);
                //            item_count++;
                //            if (Convert.ToInt32(product_id) == productId)
                //            {
                //                products = "||" + productId + "-" + item_count;
                //            }
                //        }
                //    }
                //}
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(100);
                _uow.Cookie.SetCookie("basket-item-count", pcount.ToString(), options);
                _uow.Cookie.SetCookie("product-items", products, options);
                return Json(pcount);
            }
            if (filter == "get")
            {
                return Json(_uow.Cookie.GetCookie("basket-item-count"));
            }
            return Json("no");
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public JsonResult InsertImages(int productid, IEnumerable<IFormFile> images)
        {
            _uow.Media.InsertProductImages(_uow.Product.GetById(productid), images);
            return Json("ok");
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public JsonResult PublishStatus(bool enabled, int productId)
        {
            var page = _uow.Product.GetById(productId);
            if (page != null)
            {
                page.Status = enabled;
                _uow.Product.Update(page);
                if (enabled == true)
                {
                    return Json("start");
                }
                return Json("pause");
            }
            return Json("no");
        }
        [HttpGet]
        public IActionResult List()
        {
            return View(_uow.Product.GetAllPC());
        }
        [HttpGet]
        public IActionResult DataList()
        {
            return View();
        }
        [Authorize(Policy = "Staff")]
        [HttpGet]
        public IActionResult Manage(int? id)
        {
            if (id != null)
            {
                //string sectionjsonData = @"[""]";
                //var yeniOgrenci = new
                //{
                //    caption = "Yiğit",
                //    size = "Nuhuz",
                //    width = 320,
                //    url = 50,
                //    key = 50
                //};
                //foreach (var item in _uow.Product.GetAllProductImage(Convert.ToInt32(id)))
                //{
                //    sectionjsonData.
                //}
                //JsonConvert.SerializeObject(json);

                return View(_uow.Product.GetProductById(Convert.ToInt32(id)));
            }
            return View(new Product());
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public JsonResult DeleteProductImage(int id)
        {
            if (id != 0)
            {
                var pimage = _uow.ProductImage.GetById(id);
                _uow.ProductImage.Delete(pimage);
                _uow.Media.DeleteImage(pimage.Image, "admin", "/img/product/");
                return Json("ok");
            }
            return Json("no");
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {
            return View(_uow.Product.GetProductById(Convert.ToInt32(id)));
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult Manage(Product entity, IEnumerable<IFormFile> images)
        {
            Random randomNumber = new Random();
            String serialRandom = randomNumber.Next(0, 1000000).ToString("D6");
            var item__list = _uow.Product.GetAll().Where(p => p.Barcode == serialRandom).ToList();
            if (item__list.Count() != 0)
            {
                serialRandom = randomNumber.Next(0, 1000000).ToString("D6");
            }
            if (entity.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    entity.Barcode = serialRandom;
                    entity.Status = false;
                    entity.Date = DateTime.Now;

                    _uow.Product.Add(entity);
                    return RedirectToAction("List");
                }
                return View(entity);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (entity.Barcode == null)
                    {
                        entity.Barcode = serialRandom;
                    }
                    _uow.Product.Update(entity);
                    return RedirectToAction("List");
                }
                return View(entity);
            }
        }
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(int id)
        {
            Product item = _uow.Product.GetById(id);
            _uow.Product.Delete(item);
            return Json("ok");
        }
    }
}
