using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IPageAnalysisService : IGenericService<PageAnalysis>
    {
        string GetUserIp { get; }
        string GetVisitorDataByDate(DateTime dateTime);
        string GetAllVisitorData();
        string GetVisitorDataByPage(string permalink, DateTime dateTime);
        string GetThisMonth();
        public string[] GetDataByLastWeek();
        public string[] GetDayByLastWeek();
    }
}
