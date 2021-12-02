using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewModel
{
    public class WidgetViewModel
    {
        //Totals
        public int TotalProduct { get; set; }
        public int TotalProductBrand { get; set; }
        public int TotalProductByFalse { get; set; }
        public int TotalCurrent { get; set; }
        public int TotalMessage { get; set; }
        public int TotalStaff { get; set; }
        public int TotalStaffByFalse { get; set; }
        public int TotalInvoice { get; set; }
        public int TotalCategory { get; set; }
        public int TotalDepartment { get; set; }
        public int TotalPanelVisitor { get; set; }
        public int TotalSendedMail { get; set; }
        public int TotalActiveCurrent { get; set; }
        public int ProductNotInCargo { get; set; }
        public int TotalSale { get; set; }
        public int TotalStock { get; set; }
        public int TotalMedia { get; set; }
        public int TotalVisitor { get; set; }
        public int TotalComment { get; set; }
        public int ProductInCargo { get; set; }
        public int TotalSelledProduct { get; set; }
        public int TotalSubscriber { get; set; }
        public int TodayVisitor { get; set; }
        public int TodayHomeVisitor { get; set; }
        public int TodayAdminVisitor { get; set; }
        public int TodayCurrentRegistered { get; set; }

        public string CategoryByMostSale { get; set; }
        public string StaffNameByMostSale { get; set; }
        public string ProductByMaxPrice { get; set; }
        public string CurrentByMostBuying { get; set; }
        public string ProductByMostComment { get; set; }
        public string ProductBrandByMostSale { get; set; }
        public string ProductByMostSale { get; set; }
        public string ProductByLessSale { get; set; }

    }
}
