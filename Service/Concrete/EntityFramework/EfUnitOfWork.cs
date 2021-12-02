using Service.Abstract;
using Entity.Context;
using Service.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Service.Concrete.EntityFramework
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly SwappDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _webHostEnvironment;
        private IAdminService _adminService;
        private IMessageService _messageService;
        private IEmailStatusService _emailStatusService;
        private IAnnouncementService _announcementService;
        private ICargoProcessService _cargoProcessService;
        private ICategoryService _categoryService;
        private IContactService _contactService;
        private IConfirmationEmailService _confirmationEmailService;
        private IProductImageService _productImageService;
        private ICommonService _commonService;
        private INewsletterService _newsletterService;
        private IEmailAttachmentService _emailAttachmentService;
        private INotificationService _notificationService;
        private IMessageRoomService _msgroomService;
        private IProjectService _projectService;
        private IBlogService _blogService;
        private IDashboardService _dashboardService;
        private ICurrentService _currentService;
        private IDepartmentService _departmentService;
        private IInvoiceService _invoiceService;
        private IInvoiceDetailService _invoiceDetailService;
        private IProductService _productService;
        private ISaleProcessService _saleProcessService;
        private ISearchService _searchService;
        private IStaffService _staffService;
        private IToDoListService _toDoListService;
        private IToDoListGroupService _toDoListGroupService;
        private ISettingService _settingService;
        private ICookieService _cookieService;
        private IPageAnalysisService _pageAnalysisService;
        private ILanguageService _languageService;
        public IMediaService _mediaService;
        private IAdminResPasService _adminResPas;
        private ITimeLineService _timeLineService;
        private IToDoListUserService _toDoListUserService;
        private IMenuService _menuService;
        private IProductCargoService _productCargoService;
        private ICargoCompanyService _cargoCompanyService;
        private INotificationTypeService _notificationTypeService;
        private ICalendarService _calendarService;
        private IExpenseService _expenseService;
        private IEmailService _emailService;

        public EfUnitOfWork(SwappDbContext db, IHttpContextAccessor httpContextAccessor, IHostingEnvironment webHostEnvironment)
        {
            _db = db ?? throw new ArgumentNullException("_db can not be null");
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
        public IAdminService Admin
        {
            get
            {
                return _adminService ?? (_adminService = new EfAdminService(_db, _httpContextAccessor));
            }
        }
        public IAnnouncementService Announcement
        {
            get
            {
                return _announcementService ?? (_announcementService = new EfAnnouncementService(_db));
            }
        }
        public IConfirmationEmailService ConfirmationEmail
        {
            get
            {
                return _confirmationEmailService ?? (_confirmationEmailService = new EfConfirmationEmailService(_db));
            }
        }
        public IBlogService Blog
        {
            get
            {
                return _blogService ?? (_blogService = new EfBlogService(_db));
            }
        }
        public IProjectService Project
        {
            get
            {
                return _projectService ?? (_projectService = new EfProjectService(_db));
            }
        }
        public IContactService Contact
        {
            get
            {
                return _contactService ?? (_contactService = new EfContactService(_db));
            }
        }
        public ICargoProcessService CargoProcess
        {
            get
            {
                return _cargoProcessService ?? (_cargoProcessService = new EfCargoProcessService(_db));
            }
        }
        public IMenuService Menu
        {
            get
            {
                return _menuService ?? (_menuService = new EfMenuService(_db));
            }
        }
        public IProductImageService ProductImage
        {
            get
            {
                return _productImageService ?? (_productImageService = new EfProductImageService(_db));
            }
        }
        public IEmailAttachmentService EmailAttachment
        {
            get
            {
                return _emailAttachmentService ?? (_emailAttachmentService = new EfEmailAttachmentService(_db, _mediaService));
            }
        }

        public IDashboardService Dashboard
        {
            get
            {
                return _dashboardService ?? (_dashboardService = new EfDashboardService(_db));
            }
        }

        public IEmailStatusService EmailStatus
        {
            get
            {
                return _emailStatusService ?? (_emailStatusService = new EfEmailStatusService(_db));
            }
        }
        public IToDoListUserService ToDoListUser
        {
            get
            {
                return _toDoListUserService ?? (_toDoListUserService = new EfToDoListUserService(_db));
            }
        }
        public INotificationTypeService NotificationType
        {
            get
            {
                return _notificationTypeService ?? (_notificationTypeService = new EfNotificationTypeService(_db));
            }
        }
        public ITimeLineService TimeLine
        {
            get
            {
                return _timeLineService ?? (_timeLineService = new EfTimeLineService(_db));
            }
        }
        public IMessageRoomService MessageRoom
        {
            get
            {
                return _msgroomService ?? (_msgroomService = new EfMessageRoomService(_db));
            }
        }
        public IToDoListGroupService ToDoListGroup
        {
            get
            {
                return _toDoListGroupService ?? (_toDoListGroupService = new EfToDoListGroupService(_db));
            }
        }
        public ISearchService Search
        {
            get
            {
                return _searchService ?? (_searchService = new EfSearchService(_db));
            }
        }
        public ICalendarService Calendar
        {
            get
            {
                return _calendarService ?? (_calendarService = new EfCalendarService(_db));
            }
        }
        public IEmailService Email
        {
            get
            {
                return _emailService ?? (_emailService = new EfEmailService(_db));
            }
        }
        public IMessageService Message
        {
            get
            {
                return _messageService ?? (_messageService = new EfMessageService(_db));
            }
        }
        public ISettingService Setting
        {
            get
            {
                return _settingService ?? (_settingService = new EfSettingService(_db));
            }
        }
        public IPageAnalysisService PageAnalysis
        {
            get
            {
                return _pageAnalysisService ?? (_pageAnalysisService = new EfPageAnalysisService(_db,_httpContextAccessor));
            }
        }
        public ICategoryService Category
        {
            get
            {
                return _categoryService ?? (_categoryService = new EfCategoryService(_db));
            }
        }
        public IMediaService Media
        {
            get
            {
                return _mediaService ?? (_mediaService = new EfMediaService(_db,_webHostEnvironment));
            }
        }
        public IAdminResPasService AdminResPas
        {
            get
            {
                return _adminResPas ?? (_adminResPas = new EfAdminResPasService(_db,_httpContextAccessor));
            }
        }
        public INotificationService Notification
        {
            get
            {
                return _notificationService ?? (_notificationService = new EfNotificationService(_db));
            }
        }
        public INewsletterService Newsletter
        {
            get
            {
                return _newsletterService ?? (_newsletterService = new EfNewsletterService(_db));
            }
        }
        public ILanguageService Language
        {
            get
            {
                return _languageService ?? (_languageService = new EfLanguageService(_db,_httpContextAccessor));
            }
        }
        public ICommonService Common
        {
            get
            {
                return _commonService ?? (_commonService = new EfCommonService(_db, _httpContextAccessor));
            }
        }
        public ICookieService Cookie
        {
            get
            {
                return _cookieService ?? (_cookieService = new EfCookieService(_db, _httpContextAccessor));
            }
        }
        public ICurrentService Current
        {
            get
            {
                return _currentService ?? (_currentService = new EfCurrentService(_db));
            }
        }
        public IDepartmentService Department
        {
            get
            {
                return _departmentService ?? (_departmentService = new EfDepartmentService(_db));
            }
        }
        public IExpenseService Expense
        {
            get
            {
                return _expenseService ?? (_expenseService = new EfExpenseService(_db));
            }
        }
        public IInvoiceService Invoice
        {
            get
            {
                return _invoiceService ?? (_invoiceService = new EfInvoiceService(_db, _httpContextAccessor));
            }
        }
        public IInvoiceDetailService InvoiceDetail
        {
            get
            {
                return _invoiceDetailService ?? (_invoiceDetailService = new EfInvoiceDetailService(_db));
            }
        }
        public IProductService Product
        {
            get
            {
                return _productService ?? (_productService = new EfProductService(_db));
            }
        }
        public IStaffService Staff
        {
            get
            {
                return _staffService ?? (_staffService = new EfStaffService(_db));
            }
        }
        public ISaleProcessService SaleProcess
        {
            get
            {
                return _saleProcessService ?? (_saleProcessService = new EfSaleProcessService(_db));
            }
        }
        public IToDoListService ToDoList
        {
            get
            {
                return _toDoListService ?? (_toDoListService = new EfToDoListService(_db, _httpContextAccessor));
            }
        }
        public ICargoCompanyService CargoCompany
        {
            get
            {
                return _cargoCompanyService ?? (_cargoCompanyService = new EfCargoCompanyService(_db));
            }
        }
        public IProductCargoService ProductCargo
        {
            get
            {
                return _productCargoService ?? (_productCargoService = new EfProductCargoService(_db));
            }
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
