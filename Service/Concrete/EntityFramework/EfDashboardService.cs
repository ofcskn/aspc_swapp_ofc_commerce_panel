using Entity.Context;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Service.Abstract;
using Service.Utilities;

namespace Service.Concrete.EntityFramework
{
    public class EfDashboardService : IDashboardService
    {
        private readonly SwappDbContext _db;
        public EfDashboardService(SwappDbContext db) : base()
        {

            _db = db;
        }
        public Product ProductByMost()
        {
            var p = _db.Product.OrderByDescending(p => p.Stock).First();
            return p;
        }
        public int GetTotalMessage()
        {
            var p = _db.Message.Count();
            return p;
        }
        public int GetTotalProductImage()
        {
            var p = _db.ProductImage.Count();
            return p;
        }
        public int GetTotalProductStock()
        {
            int p = _db.Product.Sum(p => Convert.ToInt32(p.Stock));
            return p;
        }
        public int GetTotalProductComment()
        {
            var p = _db.ProductComment.Count();
            return p;
        }

        public int GetTotalProductCargo(bool filter)
        {
            var p = _db.ProductCargo.Where(p => p.Enabled == filter).Count();
            return p;
        }
        public int GetTotalActiveCurrent()
        {
            var p = _db.Current.Where(p => p.Status == true).Count();
            return p;
        }
        public int GetTotalDepartment()
        {
            var p = _db.Department.Count();
            return p;
        }
        public int GetTotalVisitor()
        {
            var p = _db.PageAnalysis.Count();
            return p;
        }
        public int GetTotalPanelVisitor()
        {
            var p = _db.PageAnalysis.Where(p => p.Page.Contains("admin")).Count();
            return p;
        }
        public int GetTotalSendedEmail()
        {
            var p = _db.Email.Count();
            return p;
        }
        public int GetPCriticStock()
        {
            var p = _db.Product.Count(p => Convert.ToInt32(p.Stock) <= 30);
            return p;
        }
        public int GetBrandByProduct()
        {
            var p = (from x in _db.Product select x.Brand).Distinct().Count();
            return p;
        }
        public string GetProductByMaxPrice()
        {
            var p = (from x in _db.Product orderby x.SalePrice descending select x.Name).FirstOrDefault();
            return p;
        }
        public string GetStaffByMostSale()
        {
            var sid = _db.SaleProcess.GroupBy(p => p.StaffId).OrderByDescending(p => p.Count()).Select(p => p.Key).FirstOrDefault();
            var s = _db.Staff.FirstOrDefault(p => p.Id == sid);
            if (s != null)
            {
                return s.Name + " " + s.Surname;
            }
            return "-";
        }
        public string GetProductByMostComment()
        {
            var pid = _db.ProductComment.GroupBy(p => p.ProductId).OrderByDescending(p => p.Count()).Select(p => p.Key).FirstOrDefault();
            var s = _db.Product.FirstOrDefault(p => p.Id == pid);
            if (s != null)
            {
                return s.Name;
            }
            return "-";
        }

        public string GetCurrentByMostBuying()
        {
            var id = _db.SaleProcess.GroupBy(p => p.CurrentId).OrderByDescending(p => p.Count()).Select(p => p.Key).FirstOrDefault();
            var s = _db.Current.FirstOrDefault(p => p.Id == id);
            if (s != null)
            {
                return s.Name + " " + s.Surname;
            }
            return "-";
        }
        public string GetProductByLessSale()
        {
            var id = _db.SaleProcess.GroupBy(p => p.ProductId).OrderBy(p => p.Count()).Select(p => p.Key).FirstOrDefault();
            var s = _db.Product.FirstOrDefault(p => p.Id == id).Name;
            return s;
        }
        public string GetCategoryByMostSale()
        {
            var id = _db.SaleProcess.GroupBy(p => p.ProductId).OrderByDescending(p => p.Count()).Select(p => p.Key).FirstOrDefault();
            var s = _db.Category.FirstOrDefault(p => p.Id == id).Name;
            return s;
        }
        public string GetBrandByMostSale()
        {
            var id = _db.SaleProcess.GroupBy(p => p.ProductId).OrderByDescending(p => p.Count()).Select(p => p.Key).FirstOrDefault();
            var s = _db.Product.FirstOrDefault(p => p.Id == id).Brand;
            return s;
        }
        public int GetTodayVisitor(string panel)
        {
            int p = 0;
            if (panel == "www")
            {
                p = _db.PageAnalysis.Where(p => !p.Page.Contains("admin") && p.EntryDate.Date == DateTime.Now.Date).Count();
            }
            else if (panel == "admin")
            {
                p = _db.PageAnalysis.Where(p => p.EntryDate.Date == DateTime.Now.Date && p.Page.Contains("admin")).Count();
            }
            else
            {
                p = _db.PageAnalysis.Where(p => p.EntryDate.Date == DateTime.Now.Date).Count();
            }
            return p;
        }

        public int GetTodayCurrentRegistered()
        {
            int p = _db.Current.Where(p => p.RegisterDate.Date == DateTime.Now.Date).Count();
            return p;
        }
        public int GetTotalProductSales()
        {
            int p = _db.SaleProcess.Sum(p => Convert.ToInt32(p.Total));
            return p;
        }
        public decimal GetProfitStatusByLastMonth()
        {
            var previousMonth = DateTime.Now.AddMonths(-1);
            decimal pmsales = _db.SaleProcess.Where(p => p.Date.Month == previousMonth.Month).Sum(p => Convert.ToInt32(p.Total));
            decimal tmsales = _db.SaleProcess.Where(p => p.Date.Month == DateTime.Now.Month).Sum(p => Convert.ToInt32(p.Total));
            decimal r = (tmsales - pmsales) / 100;
            return r;
        }
        public decimal GetTVPercentByPreviousWeek()
        {
            var previousWeek = DateTime.Now.AddDays(-7);
            var previous2Week = DateTime.Now.AddDays(-14);
            decimal pw = _db.PageAnalysis.Where(p => previous2Week < p.EntryDate && p.EntryDate < previousWeek).Count();
            decimal tw = _db.PageAnalysis.Where(p => p.EntryDate > previousWeek).Count();
            decimal r = (tw - pw) * 100 / 7;
            return r;
        }
        public string[] GetTVGraphic(string filter, int ib)
        {
            string[] r = new string[ib];
            if (filter == "this-week")
            {
                var today = DateTime.Now;
                for (int i = 0; i < ib; i++)
                {
                    var day = today.AddDays(-i);
                    r[i] = _db.PageAnalysis.Where(p => p.EntryDate.Date == day.Date).Count().ToString();

                }
            }
            else if (filter == "previous-week")
            {
                var today = DateTime.Now;
                for (int i = 0; i < ib; i++)
                {
                    var day = today.AddDays((-i) - 7);
                    r[i] = _db.PageAnalysis.Where(p => p.EntryDate.Date == day.Date).Count().ToString();
                }
            }
            else if (filter == "admin")
            {
                var today = DateTime.Now;
                for (int i = 0; i < ib; i++)
                {
                    var day = today.AddDays(-i);
                    r[i] = _db.PageAnalysis.Where(p => p.EntryDate.Date == day.Date).Count().ToString();

                }
            }
            else if (filter == "www")
            {
                var today = DateTime.Now;
                for (int i = 0; i < ib; i++)
                {
                    var day = today.AddDays(-i);
                    r[i] = _db.PageAnalysis.Where(p => p.EntryDate.Date == day.Date && !p.Page.Contains("admin")).Count().ToString();

                }
            }
            else
            {
                var today = DateTime.Now;
                for (int i = 0; i < ib; i++)
                {
                    var day = today.AddDays(-i);
                    r[i] = day.ToString("M");
                }

            }
            return r;
        }
        public string[] GetLastXMonths(int ib)
        {
            string[] r = new string[ib];
            var today = DateTime.Now;
            for (int i = 0; i < ib; i++)
            {
                var day = today.AddMonths(-i);
                r[i] = day.ToString("MMMM");
            }
            return r;
        }
        public string[] GetLastXMonthsProfit(int ib)
        {
            string[] r = new string[ib];
            var today = DateTime.Now;
            for (int i = 0; i < ib; i++)
            {
                var day = today.AddMonths(-i);
                r[i] = _db.SaleProcess.Where(p => p.Date.Year == day.Year && p.Date.Month == day.Month).Sum(p => Convert.ToInt32(p.Price)).ToString();
            }
            return r;
        }
        public string[] GetLastXMonthsLoss(int ib)
        {
            string[] r = new string[ib];
            var today = DateTime.Now;
            for (int i = 0; i < ib; i++)
            {
                var day = today.AddMonths(-i);
                r[i] = _db.Product.Where(p => p.Date.Year == day.Year && p.Date.Month == day.Month).Sum(p => p.PurchasePrice).ToString();
            }
            return r;
        }
        public string[] GetAllUsedBrowsers(bool colorFilter)
        {
            //Firefox
            //Microsoft Edge
            //Yandex
            //Internet Explorer
            //Chrome
            //Apple
            //Android
            //Safari
            //Opera
            //Others


            int ib = 9;
            string[] r = new string[ib];

            for (int i = 0; i < ib; i++)
            {
                if (colorFilter == true) { r[0] = "#7D3C98"; }
                else
                {
                    r[0] = "Google Chrome";
                }
                if (colorFilter == true) { r[1] = "#8B0000"; }
                else
                {
                    r[1] = "Mozilla Firefox";
                }
                if (colorFilter == true) { r[2] = "#6495ED"; }
                else
                {
                    r[2] = "Yandex";

                }
                if (colorFilter == true) { r[3] = "#AFEEEE"; }
                else
                {
                    r[3] = "Microsoft Edge";
                }

                if (colorFilter == true) { r[4] = "#C71585"; }
                else
                {
                    r[4] = "Internet Explorer";
                }

                if (colorFilter == true) { r[5] = "#AFSTEE"; }
                else
                {
                    r[5] = "Opera";
                }
                if (colorFilter == true) { r[6] = "#117A65"; }
                else
                {
                    r[6] = "Android";
                }
                if (colorFilter == true) { r[7] = "#C0392B"; }
                else
                {
                    r[7] = "Apple";
                }
                if (colorFilter == true) { r[8] = "#2980B9"; }
                else
                {
                    r[8] = "Diğer";
                }
            }
            return r;
        }
        public int[] GetBrowserUsageDatas()
        {

            var chrome = _db.PageAnalysis.Where(p => p.Browser.Contains("Chrome") && p.Browser.Contains("(KHTML, like Gecko)") && p.Browser.Contains("Safari") && !p.Browser.Contains("Mobile")).Count();
            var microsoftEdge = _db.PageAnalysis.Where(p => p.Browser.Contains("Edg") && !p.Browser.Contains("Mobile")).Count();
            var yandex = _db.PageAnalysis.Where(p => p.Browser.Contains("Yowser") && !p.Browser.Contains("Mobile")).Count();
            var firefox = _db.PageAnalysis.Where(p => p.Browser.Contains("Firefox") && !p.Browser.Contains("Mobile")).Count();
            var explorer = _db.PageAnalysis.Where(p => p.Browser.Contains("Trident") && !p.Browser.Contains("Mobile")).Count();
            var apple = _db.PageAnalysis.Where(p => p.Browser.Contains("iPhone") && p.Browser.Contains("Mobile")).Count();
            var android = _db.PageAnalysis.Where(p => p.Browser.Contains("Android") && p.Browser.Contains("Mobile")).Count();
            var opera = _db.PageAnalysis.Where(p => p.Browser.Contains("Opera") && !p.Browser.Contains("Mobile")).Count();
            var others = _db.PageAnalysis.Count() - (chrome + microsoftEdge + yandex + firefox + android + opera + explorer + apple);
            int[] firstList = new int[9] { chrome, firefox, yandex, microsoftEdge, explorer, opera, android, apple, others };


            return firstList;
        }

        //Pie Chart 2 Languages
        public string[] GetAllUsedLanguages(bool colorFilter)
        {
            var languages = _db.PageAnalysis.GroupBy(p => p.Lang).OrderByDescending(p => p.Count()).Select(p => p.Key);
            int ib = languages.Count();
            string[] r = new string[ib];

            for (int i = 0; i < ib; i++)
            {
                if (languages.ToList()[i] != null)
                {
                    if (languages.ToList()[i] == "tr")
                    {

                        if (colorFilter == true) { r[i] = "#ff0047"; }
                        else { r[i] = "Türkçe"; }
                    }
                    else if (languages.ToList()[i] == "en")
                    {
                        if (colorFilter == true) { r[i] = "#3c8dbc"; }
                        else { r[i] = "İngilizce"; }
                    }
                    else if (languages.ToList()[i] == "de")
                    {
                        if (colorFilter == true) { r[i] = "#f012be"; }
                        else { r[i] = "Almanca"; }
                    }
                    else if (languages.ToList()[i] == "ar")
                    {
                        if (colorFilter == true) { r[i] = "#C71585"; }
                        else { r[i] = "Arapça"; }
                    }
                    else if (languages.ToList()[i] == "fr")
                    {
                        if (colorFilter == true) { r[i] = "#08b4d0"; }
                        else { r[i] = "Fransızca"; }
                    }
                    else
                    {
                        if (colorFilter == true) { r[i] = "#20c997"; }
                        else { r[i] = "Diğer"; }
                    }
                }
                else
                {
                    if (colorFilter == true) { r[i] = "#007bff"; }
                    else { r[i] = "Diğer"; }
                }
            }
            return r;
        }
        public string[] GetLanguageUsageDatas()
        {
            var languages = _db.PageAnalysis.GroupBy(p => p.Lang).OrderByDescending(p => p.Count()).Select(p => p.Key);
            int ib = languages.Count();
            string[] r = new string[ib];

            for (int i = 0; i < ib; i++)
            {
                r[i] = _db.PageAnalysis.Where(p => p.Lang == languages.ToList()[i]).Count().ToString();
            }
            return r;
        }
    }
}
