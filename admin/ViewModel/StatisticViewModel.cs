using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewModel
{
    public class StatisticViewModel
    {
        public int TotalVisitor { get; set; }
        public int TodayVisitor { get; set; }
        public int TodayHomeVisitor { get; set; }
        public int TodayAdminVisitor { get; set; }
        public int TodayCurrentRegistered { get; set; }

        //Sales Graphics
        public int STotalProductSales { get; set; }
        public decimal TVPercentByPreviousWeek { get; set; }
        public decimal SProfitStatusByLastMonth { get; set; }
        public string[] LastXMonth { get; set; }
        public string[] LastXMonthProfit { get; set; }
        public string[] LastXMonthLoss { get; set; }

        //Visitor Graphic
        public string[] TVPreviousWeek { get; set; }
        public string[] TVThisWeek { get; set; }
        public string[] LastXDay { get; set; }

        //Visitor Graphic
        public string[] Last7Day { get; set; }
        public string[] last7DayVisitorsWww { get; set; }
        public string[] last7DayVisitorsAdmin { get; set; }
        
        //Pie Chart Graphic 1
        public int[] BrowserUsageDatas { get; set; }
        public string[] AllUsedBrowsers { get; set; }
        public string[] BrowserUsageChartColors { get; set; }

        //Pie Chart Graphic 1
        public string[] AllUsedLanguages { get; set; }
        public string[] LanguageUsageDatas { get; set; }
        public string[] LanguageUsageChartColors { get; set; }

    }
}
