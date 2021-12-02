using admin.ViewModel;
using admin.ViewModels;
using AutoMapper;
using Entity.Context;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace admin.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public readonly static List<NotificationViewModel> _Notifications = new List<NotificationViewModel>();
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();
        private readonly IMapper _mapper;

        private readonly SwappDbContext _context;
        private readonly IUnitOfWork _uow;

        public NotificationHub(SwappDbContext context, IUnitOfWork uow, IMapper mapper)
        {
            _context = context;
            _uow = uow;
            _mapper = mapper;
        }
        public IEnumerable<Notification> GetNotifications()
        {
            //// First run?
            //if (_Notifications.Count == 0)
            //{
            //    foreach (var notification in _uow.Notification.GetAll())
            //    {
            //        NotificationViewModel nvm = new NotificationViewModel();
            //        nvm.Title = notification.Type;
            //        _Notifications.Add(nvm);
            //    }
            //}

            return _uow.Notification.GetAll();
        }
    }
}
