using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IAdminService Admin { get; }
        IConfirmationEmailService ConfirmationEmail { get; }
        ICategoryService Category { get; }
        ICommonService Common { get; }
        IBlogService Blog { get; }
        IProjectService Project { get; }
        ICargoProcessService CargoProcess { get; }
        INewsletterService Newsletter { get; }
        IAnnouncementService Announcement { get; }
        INotificationService Notification { get; }
        IEmailAttachmentService EmailAttachment { get; }
        IContactService Contact { get; }
        IDashboardService Dashboard { get; }
        IMessageRoomService MessageRoom { get; }
        IProductImageService ProductImage { get; }
        ICookieService Cookie { get; }
        IMenuService Menu { get; }
        IEmailStatusService EmailStatus { get; }
        ISettingService Setting { get; }
        ICurrentService Current { get; }
        IDepartmentService Department { get; }
        IInvoiceService Invoice { get; }
        IMessageService Message { get; }
        ISearchService Search { get; }
        IInvoiceDetailService InvoiceDetail { get; }
        IProductService Product { get; }
        ISaleProcessService SaleProcess { get; }
        IStaffService Staff { get; }
        IToDoListService ToDoList { get; }
        IToDoListGroupService ToDoListGroup { get; }
        IToDoListUserService ToDoListUser { get; }
        ITimeLineService TimeLine { get; }
        ICargoCompanyService CargoCompany { get; }
        IProductCargoService ProductCargo { get; }
        ICalendarService Calendar { get; }
        IEmailService Email { get; }
        IExpenseService Expense { get; }


        IAdminResPasService AdminResPas { get; }
        INotificationTypeService NotificationType { get; }
        IMediaService Media { get; }
        ILanguageService Language { get; }
        IPageAnalysisService PageAnalysis { get; }

    }
}
