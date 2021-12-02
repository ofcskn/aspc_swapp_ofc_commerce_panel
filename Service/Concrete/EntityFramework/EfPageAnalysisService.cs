
using Entity.Context;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Service.Abstract;
using Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfPageAnalysisService : EfGenericService<PageAnalysis>, IPageAnalysisService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EfPageAnalysisService(SwappDbContext _context, IHttpContextAccessor httpContextAccessor) : base(_context)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public string GetUserIp
        {
            get { return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(); }
        }
        public string GetVisitorDataByDate(DateTime dateTime)
        {
            if (dateTime != null)
            {
                var analysis = _db.PageAnalysis.Where(p => p.EntryDate.Day == dateTime.Day);
                string pageCount = Convert.ToString(analysis.Count());
                return pageCount;
            }
            return "öfc";
        }
        public string[] GetDataByLastWeek()
        {
            string[] dayDataList = new string[7];
            var today = DateTime.Now;
            for (int i = 0; i < 7; i++)
            {
                var analysis = _db.PageAnalysis.Where(p => p.EntryDate.Day == today.AddDays(Convert.ToDouble(-i)).Day);
                string pageCount = Convert.ToString(analysis.Count());
                dayDataList[i] = pageCount;
            }
            return dayDataList;
        }
        //public string[] GetPagesByLang(string lang)
        //{
        //    int count = _db.Page.Where(p => p.Lang == lang).Count();
        //    string[] pageDataList = new string[count];
        //    var today = DateTime.Now;
        //    for (int i = 0; i < 7; i++)
        //    {
        //        var analysis = _db.PageAnalysis.Where(p => p.EntryDate.Day == today.AddDays(Convert.ToDouble(-i)).Day);
        //        string pageCount = Convert.ToString(analysis.Count());
        //        dayDataList[i] = pageCount;
        //    }
        //    return dayDataList;
        //}
        //public string[] GetPagesByLang(string lang)
        //{
        //    var pages = _db.Page.Where(p => p.Lang == lang);
        //    int count = _db.Page.Where(p => p.Lang == lang).Count();
        //    string[] pageList = new string[count];
        //    var today = DateTime.Now;
        //    for (int i = 0; i < count; i++)
        //    {
        //        pageList[i] = pages[i];
        //    }
        //    return pageList;
        //}
        public string[] GetDayByLastWeek()
        {
            string[] dayList = new string[7];
            var today = DateTime.Now;
            for (int i = 0; i < 7; i++)
            {
                if (i == 0)
                {
                    dayList[i] = "Bugün";
                }
                else if (i == 1)
                {
                    dayList[i] = "Dün";
                }
                else
                {
                    dayList[i] = today.AddDays(Convert.ToDouble(-i)).ToString("dddd");
                }
            }
            return dayList;
        }
        public string GetVisitorDataByPage(string permalink, DateTime dateTime)
        {
            if (dateTime != null)
            {
                var analysis = _db.PageAnalysis.Where(p => p.EntryDate.Day == dateTime.Day && p.Page == permalink).GroupBy(grp => grp.Ip).Where(p => p.Count() >= 1).Select(grp => grp.Key);
                string pageCount = Convert.ToString(analysis.Count());
                return pageCount;
            }
            return "öfc";
        }
        //public int GetSubscriberCount(DateTime thisMonth)
        //{
        //    return _db.Newsletter.ToList().Where(p => Convert.ToDateTime(p.Date).Month == thisMonth.Month).Count();
        //}
        public string GetThisMonth()
        {
            return DateTime.Now.ToString("MMMM");
        }

        //public int GetContactByMonth()
        //{
        //    return _db.Contact.ToList().Where(p => Convert.ToDateTime(p.Date).Month == DateTime.Now.Month).Count();
        //}

        public string GetAllVisitorData()
        {
            //string[] dayDataList = new string[_db.PageAnalysis.Where(p=>p.EntryDate == )];
            //var today = DateTime.Now;
            //for (int i = 0; i < 7; i++)
            //{
            //    var analysis = _db.PageAnalysis.Where(p => p.EntryDate.Day == today.AddDays(Convert.ToDouble(-i)).Day).GroupBy(grp => grp.Ip).Where(p => p.Count() >= 1).Select(grp => grp.Key);
            //    string pageCount = Convert.ToString(analysis.Count());
            //    dayDataList[i] = pageCount;
            //}
            //return dayDataList;
            return "sdasd";
        }


    }
}
