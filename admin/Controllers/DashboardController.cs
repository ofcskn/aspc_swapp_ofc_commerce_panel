using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using admin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace admin.Controllers
{
    [Authorize(Policy = "User")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _uow;
        public DashboardController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public IActionResult Index()
        {
            StatisticViewModel svm = new StatisticViewModel();
            svm.TodayCurrentRegistered = _uow.Dashboard.GetTodayCurrentRegistered();
            svm.TodayVisitor = _uow.Dashboard.GetTodayVisitor("all");
            svm.TodayHomeVisitor = _uow.Dashboard.GetTodayVisitor("www");
            svm.TodayAdminVisitor = _uow.Dashboard.GetTodayVisitor("/admin");
            svm.TotalVisitor = _uow.Dashboard.GetTotalVisitor();
            //Sales Graphics
            svm.STotalProductSales = _uow.Dashboard.GetTotalProductSales();
            svm.SProfitStatusByLastMonth = _uow.Dashboard.GetProfitStatusByLastMonth();
            svm.LastXMonth = _uow.Dashboard.GetLastXMonths(6);
            svm.LastXMonthProfit = _uow.Dashboard.GetLastXMonthsProfit(6);
            svm.LastXMonthLoss = _uow.Dashboard.GetLastXMonthsLoss(6);
            svm.TVPercentByPreviousWeek = _uow.Dashboard.GetTVPercentByPreviousWeek();
            svm.TVPreviousWeek = _uow.Dashboard.GetTVGraphic("previous-week",7);
            svm.TVThisWeek = _uow.Dashboard.GetTVGraphic("this-week", 7);
            svm.LastXDay = _uow.Dashboard.GetTVGraphic("-", 7);
            //Pie Chart 1 (About Browser)
            svm.AllUsedBrowsers = _uow.Dashboard.GetAllUsedBrowsers(false);
            svm.BrowserUsageDatas = _uow.Dashboard.GetBrowserUsageDatas();
            svm.BrowserUsageChartColors = _uow.Dashboard.GetAllUsedBrowsers(true);
            //Pie Chart 2 (About Lang)
            svm.AllUsedLanguages = _uow.Dashboard.GetAllUsedLanguages(false);
            svm.LanguageUsageDatas = _uow.Dashboard.GetLanguageUsageDatas();
            svm.LanguageUsageChartColors = _uow.Dashboard.GetAllUsedLanguages(true);
            return View(svm);
        }
        public IActionResult Widgets()
        {
            WidgetViewModel w = new WidgetViewModel();
            w.TotalProduct = _uow.Product.GetAll().Count();
            w.TotalProductByFalse = _uow.Product.GetAll().Where(p=>p.Status == false).Count();
            w.TotalInvoice = _uow.Invoice.GetAll().Count();
            w.TotalMedia = _uow.Media.GetAll().Count();
            w.TotalMessage = _uow.Dashboard.GetTotalMessage();
            w.TotalComment = _uow.Dashboard.GetTotalProductComment();
            w.TotalCategory = _uow.Category.GetAll().Count();
            w.TotalSendedMail = _uow.Dashboard.GetTotalSendedEmail();
            w.ProductInCargo = _uow.Dashboard.GetTotalProductCargo(true);
            w.ProductNotInCargo = _uow.Dashboard.GetTotalProductCargo(false);
            w.TotalActiveCurrent = _uow.Dashboard.GetTotalActiveCurrent();
            w.TotalCurrent = _uow.Current.GetAll().Count();
            w.TotalPanelVisitor = _uow.Dashboard.GetTotalPanelVisitor();
            w.TotalVisitor = _uow.Dashboard.GetTotalVisitor();
            w.TotalStock = _uow.Dashboard.GetTotalProductStock();
            w.TotalDepartment = _uow.Dashboard.GetTotalDepartment();
            w.TotalProductBrand = _uow.Dashboard.GetBrandByProduct();
            w.ProductByMaxPrice = _uow.Dashboard.GetProductByMaxPrice();
            w.StaffNameByMostSale = _uow.Dashboard.GetStaffByMostSale();
            w.CurrentByMostBuying = _uow.Dashboard.GetCurrentByMostBuying();
            w.ProductByMostComment = _uow.Dashboard.GetProductByMostComment();
            w.CategoryByMostSale = _uow.Dashboard.GetCategoryByMostSale();
            w.ProductByLessSale = _uow.Dashboard.GetProductByLessSale();
            w.ProductBrandByMostSale = _uow.Dashboard.GetBrandByMostSale();
            w.TodayCurrentRegistered = _uow.Dashboard.GetTodayCurrentRegistered();
            w.TodayVisitor = _uow.Dashboard.GetTodayVisitor(null);
            w.TodayHomeVisitor = _uow.Dashboard.GetTodayVisitor("/");
            w.TodayAdminVisitor = _uow.Dashboard.GetTodayVisitor("/admin");
            w.TotalSale = _uow.SaleProcess.GetAll().Count();
            w.TotalStaff = _uow.Staff.GetAll().Count();
            w.TotalStaffByFalse = _uow.Staff.GetAll().Where(p=>p.Status == false).Count();
            //w.TotalStock = _uow.Product.GetAll().Where(p=>p.Status == false).Count();
            w.ProductByMostSale = _uow.Dashboard.ProductByMost().Name;
            w.ProductByMostSale = _uow.Dashboard.ProductByMost().Name;
            return View(w);
        }
        public IActionResult Charts()
        {
            StatisticViewModel svm = new StatisticViewModel();
            svm.TodayCurrentRegistered = _uow.Dashboard.GetTodayCurrentRegistered();
            svm.TodayVisitor = _uow.Dashboard.GetTodayVisitor("all");
            svm.TodayHomeVisitor = _uow.Dashboard.GetTodayVisitor("www");
            svm.TodayAdminVisitor = _uow.Dashboard.GetTodayVisitor("/admin");
            svm.TotalVisitor = _uow.Dashboard.GetTotalVisitor();
            //Sales Graphics
            svm.STotalProductSales = _uow.Dashboard.GetTotalProductSales();
            svm.SProfitStatusByLastMonth = _uow.Dashboard.GetProfitStatusByLastMonth();
            svm.LastXMonth = _uow.Dashboard.GetLastXMonths(6);
            svm.LastXMonthProfit = _uow.Dashboard.GetLastXMonthsProfit(6);
            svm.LastXMonthLoss = _uow.Dashboard.GetLastXMonthsLoss(6);
            svm.TVPercentByPreviousWeek = _uow.Dashboard.GetTVPercentByPreviousWeek();
            svm.TVPreviousWeek = _uow.Dashboard.GetTVGraphic("previous-week", 7);
            svm.TVThisWeek = _uow.Dashboard.GetTVGraphic("this-week", 7);
            svm.LastXDay = _uow.Dashboard.GetTVGraphic("-", 7);
            //Graphic 3
            svm.Last7Day = _uow.Dashboard.GetTVGraphic("-", 7);
            svm.last7DayVisitorsAdmin = _uow.Dashboard.GetTVGraphic("admin", 7);
            svm.last7DayVisitorsWww = _uow.Dashboard.GetTVGraphic("www", 7);
            return View(svm);
        }
        public IActionResult PieCharts()
        {
            StatisticViewModel svm = new StatisticViewModel();
            //Pie Chart 1 (About Browser)
            svm.AllUsedBrowsers = _uow.Dashboard.GetAllUsedBrowsers(false);
            svm.BrowserUsageDatas = _uow.Dashboard.GetBrowserUsageDatas();
            svm.BrowserUsageChartColors = _uow.Dashboard.GetAllUsedBrowsers(true);
            //Pie Chart 2 (About Lang)
            svm.AllUsedLanguages = _uow.Dashboard.GetAllUsedLanguages(false);
            svm.LanguageUsageDatas = _uow.Dashboard.GetLanguageUsageDatas();
            svm.LanguageUsageChartColors = _uow.Dashboard.GetAllUsedLanguages(true);
            return View(svm);
        }
    }
}
