using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IDashboardService
    {
        Product ProductByMost();
        int GetTotalMessage();
        int GetTotalProductImage();
        int GetTotalProductComment();
        int GetTotalProductCargo(bool filter);
        int GetTotalProductStock();
        int GetTotalDepartment();
        int GetTotalVisitor();
        int GetTotalPanelVisitor();
        int GetTotalActiveCurrent();
        int GetTotalSendedEmail();
        int GetBrandByProduct();
        int GetPCriticStock();
        int GetTodayVisitor(string panel);
        int GetTodayCurrentRegistered();

        string GetProductByMaxPrice();
        string GetProductByLessSale();
        string GetCategoryByMostSale();
        string GetBrandByMostSale();

        string GetStaffByMostSale();
        string GetProductByMostComment();
        string GetCurrentByMostBuying();

        //Sales Graphics
        int GetTotalProductSales();
        decimal GetTVPercentByPreviousWeek();
        decimal GetProfitStatusByLastMonth();
        string[] GetLastXMonths(int i);
        string[] GetLastXMonthsProfit(int i);
        string[] GetLastXMonthsLoss(int i);
        string[] GetTVGraphic(string filter, int ib);
        
        //Pie Chart 1
        string[] GetAllUsedBrowsers(bool colorFilter);
        int[] GetBrowserUsageDatas();
        
        //Pie Chart 2
        string[] GetAllUsedLanguages(bool colorFilter);
        string[] GetLanguageUsageDatas();
    }
}
