using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewModel
{
    public class DashboardViewModel
    {
        public IEnumerable<ToDoList> ToDoLists { get; set; }
        public int TotalVisitor { get; set; }
        public int TodayVisitor { get; set; }
        public int TotalSubscriber { get; set; }
        public int TodayHomeVisitor { get; set; }
        public int TotalSalesForToday { get; set; }
        public int NewLoginForCurrent { get; set; }
        public int NewMemberForWeek { get; set; }
        public int TodayAdminVisitor { get; set; }
        public int TodayCurrentRegistered { get; set; }

        public int GettingProducts { get; set; }
        public int SellingProducts { get; set; }
        public int GettingSellingPercent { get; set; }


        //LatestProducts
        public IEnumerable<Product> AllProducts { get; set; }
        public IEnumerable<Product> LatestProducts { get; set; }
        public IEnumerable<Staff> LatestStaff { get; set; }
        public IEnumerable<SaleProcess> LatestSaleProcess { get; set; }
        
    }
}
