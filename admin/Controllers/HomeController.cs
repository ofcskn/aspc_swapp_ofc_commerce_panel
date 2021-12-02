using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using admin.Models;
using System.Security.Claims;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Service.Abstract;
using Microsoft.AspNetCore.Authentication;
using Service.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using admin.ViewModel;

namespace admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IViewRenderService _viewRenderService;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork uow, IViewRenderService viewRenderService, IConfiguration configuration)
        {
            _logger = logger;
            _uow = uow;
            _viewRenderService = viewRenderService;
            _configuration = configuration;
        }


        public JsonResult GetLastLoginDate()
        {
            return Json(_uow.Admin.GetLastLoginDate);
        }
        [Authorize(Policy = "Current")]
        public IActionResult Current()
        {
            var id = _uow.Admin.GetIdByAdmin;
            var role = _uow.Admin.GetRoleByAdmin;
            var announcements = new List<AnnouncementAdminViewModel>();
            foreach (var item in _uow.Announcement.GetAllByEnabledDate(true))
            {
                var userId = item.AnnouncerId;
                var userRole = item.AnnouncerRole;

                AnnouncementAdminViewModel avm = new AnnouncementAdminViewModel();
                if (userRole == "admin" || userRole == "user")
                {
                    avm.Announcer = _uow.Admin.GetById(userId).Name + " " + _uow.Admin.GetById(userId).Surname;
                    avm.Avatar = _uow.Admin.GetById(userId).Image;
                }
                if (userRole == "current")
                {
                    avm.Announcer = _uow.Current.GetById(userId).Name + " " + _uow.Current.GetById(userId).Surname;
                    avm.Avatar = _uow.Current.GetById(userId).Image;
                }
                if (userRole == "staff")
                {
                    avm.Announcer = _uow.Staff.GetById(userId).Name + " " + _uow.Staff.GetById(userId).Surname;
                    avm.Avatar = _uow.Staff.GetById(userId).Image;
                }
                avm.Id = item.Id;
                avm.AnnouncerId = item.AnnouncerId;
                avm.AnnouncerRole = item.AnnouncerRole;
                avm.ReadNumber = item.ReadNumber;
                avm.Enabled = item.Enabled;
                avm.Date = Convert.ToDateTime(item.Date);
                avm.Title = item.Title;
                avm.Description = item.Description;
                announcements.Add(avm);
            }

            Random randomNumber = new Random();
            String serialRandom = randomNumber.Next(0, 1000000).ToString("D6");
            CurrentProfileViewModel vm = new CurrentProfileViewModel
            {
                Current = _uow.Current.GetByIdCurrent(_uow.Admin.GetIdByAdmin),
                Timelines = _uow.TimeLine.GetAllTL().Where(p => p.MemberId == id && p.MemberRole == role),
                Announcements = announcements,
                EmailCount = _uow.Email.GetAllReceivedMail(id, role).Count(),
                Score = Convert.ToInt32(serialRandom),
                NotificationCount = _uow.Notification.GetAllByUser(id, role).Count()
            };
            return View(vm);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Confirmation()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Confirmation(string code)
        {
            if (code != null)
            {
                var confEmail = _uow.ConfirmationEmail.GetAll().FirstOrDefault(p => p.ConfirmationCode == code);
                
                if (confEmail != null)
                {
                    var admin = _uow.Admin.GetById(confEmail.AdminId);
                    var current = _uow.Current.GetById(confEmail.CurrentId);
                    var staff = _uow.Staff.GetById(confEmail.StaffId);
                    if (admin != null)
                    {
                        admin.Enabled = true;
                        _uow.Admin.Update(admin);
                    }
                    if (current != null)
                    {
                        current.Status = true;
                        _uow.Current.Update(current);
                    }
                    if (staff != null)
                    {
                        staff.Status = true;
                        _uow.Staff.Update(staff);
                    }
                    ViewBag.SuccessMessage = "Kaydınız onaylanmıştır. Sisteme bilgilerinizle giriş yapabilirsiniz.";
                    return View();
                }
                else
                {
                    ViewBag.ErrorMessage = "Kodunuz hatalı. Lütfen geçerli bir kod giriniz.";
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Lütfen kodunuzu giriniz";
                return View();
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(Admin entity, string passwordAgain)
        {
            if (ModelState.IsValid)
            {
                string email = entity.Email.ToLower();
                if (_uow.Admin.ControlUserEmailForAll(email))
                {
                    if (_uow.Admin.ControlUserNameForAll(entity.UserName))
                    {
                        if (entity.Password == passwordAgain)
                        {
                            var password = entity.Password;

                            #region adminAccount
                            entity.Role = "user";
                            entity.Email = email;
                            entity.RegisterDate = DateTime.Now;
                            entity.Enabled = false;
                            entity.LastLoginIp = _uow.Admin.GetIp();
                            entity.LastLoginDate = DateTime.Now;
                            entity.Password = Cipher.Encrypt(entity.Password, entity.UserName);
                            _uow.Admin.Add(entity);
                            #endregion

                            #region currentAccount
                            Random randomNumber = new Random();
                            String serialRandom = randomNumber.Next(0, 1000000).ToString("D6");
                            if (_uow.Current.GetAll().Any(p => p.No == serialRandom))
                            {
                                serialRandom = randomNumber.Next(0, 1000000).ToString("D6");
                            }
                            string currentNo = serialRandom;
                            string currentUserName = "cari_" + entity.UserName;
                            Current currentAccount = new Current
                            {
                                No = currentNo,
                                UserName = currentUserName,
                                Mail = "cari_" + email,
                                Name = entity.Name,
                                Surname = entity.Surname,
                                RegisterIp = _uow.Admin.GetIp(),
                                Password = Cipher.Encrypt(passwordAgain, currentUserName),//admin password
                                RegisterDate = DateTime.Now,
                                PinCode = entity.PinCode
                            };
                            _uow.Current.Add(currentAccount);
                            #endregion

                            #region staffAccount
                            string staffUserName = "personel_" + entity.UserName;
                            Staff staffAccount = new Staff
                            {
                                UserName = staffUserName,
                                Email = "personel_" + email,
                                Name = entity.Name,
                                Surname = entity.Surname,
                                StartDate = DateTime.Now,
                                LastLoginDate = DateTime.Now,
                                LastLoginIp = _uow.Admin.GetIp(),
                                Priority = _uow.Staff.GetAll().Max(p => p.Priority) + 1,
                                Password = Cipher.Encrypt(passwordAgain, staffUserName),//admin password
                                PinCode = entity.PinCode,
                            };
                            _uow.Staff.Add(staffAccount);
                            #endregion

                            #region confirmEmail
                            string confirmText = Cipher.Encrypt("confirmation", entity.Email).Replace("/", "-").Replace("+", "-");
                            string domain = _configuration.GetSection("WebSite").GetSection("Domain").Value;
                            string subscriptionLink = _configuration.GetSection("WebSite").GetSection("DomainAdmin").Value + "/confirmation/";

                            var confirmationCode = entity.UserName + "-" + serialRandom;

                            ConfirmationEmail ce = new ConfirmationEmail
                            {
                                ConfirmId = entity.Id,
                                AdminEmail = email,
                                SendDate = DateTime.Now,
                                AdminId = entity.Id,
                                CurrentId = currentAccount.Id,
                                StaffId = staffAccount.Id,
                                ConfirmationCode = confirmationCode
                            };
                            _uow.ConfirmationEmail.Add(ce);

                            ConfirmEmailViewModel html = new ConfirmEmailViewModel
                            {
                                ReceiverMail = entity.Email,
                                SubscribeDate = entity.RegisterDate,
                                SenderNameSurname = "Ömer Faruk Coşkun ~ ÖFC",
                                SenderWelcomeText = "Kayıt işlemini tamamlamak için lütfen linke tıklayın!",
                                SenderMessage = "Aşağıdaki hesaplarla sisteme çeşitli rollerde giriş yapabilirsiniz. Giriş bilgileriniz aşağıdadır. Linke tıklayarak üyeliklerinizi onayladıktan sonra sisteme kullanıcı adı ve şifrelerinizle giriş yapabilirsiniz. İyi eğlenceler. Eğer onaylama sırasında bir sorunla karşılaşırsanız <a href='" + domain + "/admin/home/confirmation'><b> ÜYELİK ONAYLA </b></a> adresine <b>" + ce.ConfirmationCode + "</b> kodu ile üyeliğinizi onaylayarak giriş yapabilirsiniz.",
                                SenderLink = domain,
                                CurrentText = "Müşteri Yetkisi İçin;",
                                CurrentUserName = currentAccount.UserName,
                                StaffText = "Personel Yetkisi İçin;",
                                UserPassword = password,
                                CurrentPassword = password,
                                StaffPassword = password,
                                StaffUserName = staffAccount.UserName,
                                UserText = "Kullanıcı Yetkisi İçin;",
                                UserName = entity.UserName,
                                ConfirmLink = subscriptionLink + ce.Id + "/" + confirmText,
                                UnsubscribeLink = "",
                                ConfirmationCode = confirmationCode
                            };
                            var result = await _viewRenderService.RenderToStringAsync("EmailDesign/EmailDesignNewsletter", html);

                            _uow.Common.SendMailToAnother("Swappo Ticari Otomasyon Üyelik Onaylama", result, "Sisteme giriş yapabilmek bu kodu ilgili bölüme girin.", _configuration, entity.Email);

                            #endregion
                            ViewBag.SuccessMessage = "Bilgileriniz başarıyla gönderilmiştir.<b>" + email + "</b> e-posta adresine gönderdiğimiz linkten kaydınızı onaylayarak sisteme giriş yapabilirsiniz. İlginiz için teşekküler.<b> Spam klasörüne bakmayı unutmayın.</b> - öfc";
                            return View();
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Şifreleriniz eşleşmiyor. Lütfen tekrar deneyin.";
                            return View(entity);
                        }
                    }
                    ViewBag.ErrorMessage = "Lütfen farklı bir kullanıcı adı ile kayıt olmayı deneyiniz.";
                    return View(entity);
                }
                ViewBag.ErrorMessage = "Lütfen farklı bir e-posta adresi ile kayıt olmayı deneyiniz.";
                return View(entity);
            }
            ViewBag.ErrorMessage = "Lütfen gerekli bilgileri doldurunuz.";
            return View(entity);
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult LockScreen(string returnUrl)
        {
            if (_uow.Cookie.GetCookie(Cipher.Encrypt("UserId", "Swapp")) == null || _uow.Cookie.GetCookie(Cipher.Encrypt("UserRole", "Swapp")) == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.returnUrl = returnUrl;
            HttpContext.SignOutAsync();

            ViewBag.UserId = Cipher.Decrypt(_uow.Cookie.GetCookie(Cipher.Encrypt("UserId", "Swapp")), "UserId");
            ViewBag.UserRole = Cipher.Decrypt(_uow.Cookie.GetCookie(Cipher.Encrypt("UserRole", "Swapp")), "UserRole");
            return View();
        }
        [Authorize(Policy = "Staff")]
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                DashboardViewModel vm = new DashboardViewModel();
                vm.TodayCurrentRegistered = _uow.Dashboard.GetTodayCurrentRegistered();
                vm.TodayVisitor = _uow.Dashboard.GetTodayVisitor("all");
                vm.TodayHomeVisitor = _uow.Dashboard.GetTodayVisitor("www");
                vm.TodayAdminVisitor = _uow.Dashboard.GetTodayVisitor("/admin");
                vm.TotalVisitor = _uow.Dashboard.GetTotalVisitor();
                //Swappo 2 Cards
                vm.TotalSalesForToday = _uow.SaleProcess.GetAll().Where(p => p.Date.Date == DateTime.Now.Date).Count();
                vm.NewMemberForWeek = _uow.Current.GetAll().Where(p => p.RegisterDate > DateTime.Now.AddDays(-7)).Count();
                vm.NewLoginForCurrent = _uow.Current.GetAll().Where(p => p.LastLoginDate.Date == DateTime.Now.Date).Count();
                //ToDoList
                vm.ToDoLists = _uow.ToDoList.GetAllByEnabled(_uow.Admin.GetIdByAdmin, _uow.Admin.GetRoleByAdmin, false).Where(p => p.GroupId == null).Take(10);
                //Products
                vm.GettingProducts = _uow.Dashboard.GetTotalProductStock();
                vm.SellingProducts = _uow.SaleProcess.GetAll().Sum(p => Convert.ToInt32(p.Amount));
                vm.LatestProducts = _uow.Product.GetAllPC().Where(p => p.Status == true).OrderByDescending(p => p.Date).Take(10);
                vm.AllProducts = _uow.Product.GetAllPC();
                //Staff
                vm.LatestStaff = _uow.Staff.GetAllByDate().Where(p => p.Status == true).OrderByDescending(p => p.StartDate).Take(12);
                //LatestSaleProcess
                vm.LatestSaleProcess = _uow.SaleProcess.GetAllByDate().Where(p => p.Status == true).OrderByDescending(p => p.Date).Take(12);

                return View("index", vm);
            }
            else
            {
                return View("login");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]//Güvenlik Açısından önemli
        public async Task<IActionResult> Login(string username, string password, string returnUrl, int remember)
        {
            var enabledErrorMessage = "Üyeliğiniz aktif değildir. Lütfen sayfa sahibi ile iletişime geçin.<b>@ofcskn</b> <br> Veya <a class='btn text-light' href='/home/contact/'>Mesaj Gönderin</a>";
            if (username != null && password != null)
            {
                HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
                Admin admin = _uow.Admin.GetAdmin(username, password);
                Current current = _uow.Current.GetCurrent(username, password);
                Staff staff = _uow.Staff.GetStaff(username, password);

                if (admin != null)
                {
                    if (admin.Enabled == true)
                    {
                        Claim[] claims = new[] {
                    new Claim("id", admin.Id.ToString()),
                    new Claim("name",  admin.Name + " " + admin.Surname),
                    new Claim("image",  admin.Image == null ? "default.jpg" :admin.Image),
                    new Claim("isAdmin", admin.IsAdmin.ToString()),
                    new Claim("role", admin.Role),
                    new Claim("lastLoginDate", admin.LastLoginDate.ToString()),
                    new Claim("lastLoginIp", admin.LastLoginIp),
                    new Claim("mail", admin.Email),
                    new Claim("username", admin.UserName),
                };
                        ClaimsIdentity identity = new ClaimsIdentity(claims, admin.Name);
                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                        if (admin.Role.Contains("user"))
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                        }
                        else
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                        }
                        admin.LastLoginIp = _uow.Admin.GetIp();
                        admin.LastLoginDate = DateTime.Now;
                        _uow.Admin.Update(admin);

                        await HttpContext.SignInAsync(principal, new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddYears(1)
                        });
                        _uow.Cookie.SetCookie(Cipher.Encrypt("UserId", "Swapp"), Cipher.Encrypt(admin.Id.ToString(), "UserId"), new CookieOptions { Expires = DateTime.Now.AddDays(14) });
                        _uow.Cookie.SetCookie(Cipher.Encrypt("UserRole", "Swapp"), Cipher.Encrypt(admin.Role.ToString(), "UserRole"), new CookieOptions { Expires = DateTime.Now.AddDays(14) });
                        if (returnUrl != null)
                        {
                            return Redirect(returnUrl ?? "/");
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = enabledErrorMessage;
                        return View();
                    }
                }
                else if (current != null)
                {
                    if (current.Status == true)
                    {
                        Claim[] claims = new[] {
                    new Claim("image", current.Image == null ? "default.jpg" :current.Image),
                    new Claim("id", current.Id.ToString()),
                    new Claim("name",  current.Name + " "+ current.Surname),
                    new Claim("mail", current.Mail),
                    new Claim("role", "current"),
                    new Claim("lastLoginDate", current.LastLoginDate.ToString()),
                    new Claim("lastLoginIp", current.LastLoginIp),
                    new Claim("username", current.UserName),
                };
                        ClaimsIdentity identity = new ClaimsIdentity(claims, current.Name);
                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                        current.LastLoginDate = DateTime.Now;
                        current.LastLoginIp = _uow.Admin.GetIp();
                        _uow.Current.Update(current);
                        identity.AddClaim(new Claim(ClaimTypes.Role, "current"));


                        _uow.Cookie.SetCookie(Cipher.Encrypt("UserId", "Swapp"), Cipher.Encrypt(current.Id.ToString(), "UserId"), new CookieOptions { Expires = DateTime.Now.AddDays(14) });
                        _uow.Cookie.SetCookie(Cipher.Encrypt("UserRole", "Swapp"), Cipher.Encrypt("current", "UserRole"), new CookieOptions { Expires = DateTime.Now.AddDays(14) });

                        await HttpContext.SignInAsync(principal, new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddYears(1)
                        });
                        if (returnUrl != null)
                        {
                            return Redirect(returnUrl ?? "/");
                        }
                        return RedirectToAction("Current");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = enabledErrorMessage;
                        return View();
                    }

                }
                else if (staff != null)
                {
                    if (staff.Status == true)
                    {
                        Claim[] claims = new[] {
                    new Claim("id", staff.Id.ToString()),
                    new Claim("name",  staff.Name + " " + staff.Surname),
                    new Claim("mail",  staff.Email),
                    new Claim("role", "staff"),
                    new Claim("lastLoginDate", staff.LastLoginDate.ToString()),
                    new Claim("lastLoginIp", staff.LastLoginIp),
                    new Claim("image", staff.Image == null ? "default.jpg" :staff.Image),
                    new Claim("username", staff.UserName),
                };
                        ClaimsIdentity identity = new ClaimsIdentity(claims, staff.Name);
                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                        identity.AddClaim(new Claim(ClaimTypes.Role, "staff"));

                        staff.LastLoginDate = DateTime.Now;
                        staff.LastLoginIp = _uow.Admin.GetIp();
                        await HttpContext.SignInAsync(principal, new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddYears(1)
                        });
                        _uow.Staff.Update(staff);

                        _uow.Cookie.SetCookie(Cipher.Encrypt("UserId", "Swapp"), Cipher.Encrypt(staff.Id.ToString(), "UserId"), new CookieOptions { Expires = DateTime.Now.AddDays(14) });
                        _uow.Cookie.SetCookie(Cipher.Encrypt("UserRole", "Swapp"), Cipher.Encrypt("staff", "UserRole"), new CookieOptions { Expires = DateTime.Now.AddDays(14) });

                        if (returnUrl != null)
                        {
                            return Redirect(returnUrl ?? "/");
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = enabledErrorMessage;
                        return View();
                    }

                }
                else if (admin == null || current == null || staff == null)
                {
                    ViewBag.ErrorMessage = "Girmiş olduğunuz bilgiler birbirleriyle uyuşmuyor. Lütfen tekrar deneyiniz.";
                    return View();
                }
            }
            ViewBag.ErrorMessage = "Lütfen gerekli alanları doldurunuz.";
            ViewBag.UserName = username;
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RememberPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]//Güvenlik Açısından önemli
        public async Task<IActionResult> RememberPassword(RememberPassword entity)
        {
            var userName = _uow.Admin.GetUserNameByEmail(entity.Email);
            if (ModelState.IsValid)
            {
                if (userName != null)
                {
                    Admin admin = _uow.Admin.GetAdminByRememberForm(entity.Email, Convert.ToInt32(entity.PinCode));
                    if (admin != null)
                    {
                        string userNameSurname = admin.Name + admin.Surname;
                        DateTime datetime = DateTime.Now;
                        string userResetLink = Cipher.Encrypt(datetime.ToString(), entity.Email);
                        string newResetLink = userResetLink.Replace("/", "-").Replace("+", "-");
                        AdminResetPassword adminResPas = new AdminResetPassword
                        {
                            SendDate = datetime,
                            AdminId = admin.Id,
                            Enabled = true,
                            Ip = _uow.AdminResPas.GetUserIp,
                            ExpireTime = datetime.AddDays(1),
                        };
                        _uow.AdminResPas.Add(adminResPas);

                        string domainAdmin = _configuration.GetSection("WebSite").GetSection("DomainAdmin").Value;
                        string passwordChangeLink = domainAdmin + "/home/resetpassword/" + adminResPas.Id + "/" + newResetLink;
                        ResetPassword rp = new ResetPassword
                        {
                            NameSurname = userNameSurname,
                            PasswordLink = passwordChangeLink
                        };
                        var html = await _viewRenderService.RenderToStringAsync("EmailDesign/_ResetPassword", rp);
                        _uow.AdminResPas.SendResetPasswordEmail("Şifre Sıfırlama", html, "Şifreniz sıfırlanacaktır.", _configuration, admin, admin.UserName, admin.Email, passwordChangeLink);
                        ViewBag.SuccessMessage = " <p>Şifre sıfırlama bağlantınız <b>" + entity.Email + "</b> mail adresine gönderildi. Bağlantıya tıklayarak şifrenizi sıfırlayabilirsiniz. Bağlantının 24 saat içinde geçerli olduğunu unutmayın.</p>";
                        return View(entity);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Girdiğin değerler hatalı. Lütfen farklı bilgiler ile tekrar deneyin.";
                        return View(entity);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Girmiş olduğunuz e-posta adresi sistemimizde kayıtlı değildir.";
                    return View(entity);
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Lütfen gerekli bölümleri doldurun.";
                return View(entity);
            }
        }
        [HttpGet]
        [Route("/home/resetpassword/{respasid}/{link}")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string link, string respasid)
        {
            AdminResetPassword respas = _uow.AdminResPas.GetById(Convert.ToInt32(respasid));
            var dayDifference = Convert.ToDateTime(respas.ExpireTime).Subtract(DateTime.Now).Hours;
            if (dayDifference >= 0 && respas.Enabled != false)
            {
                ViewBag.adminResPasId = respasid;
                ViewBag.enabled = 1;
                return View();
            }
            else
            {
                ViewBag.enabled = 0;
                return View();
            }

        }
        [HttpPost]
        [Route("/home/resetpassword/{respasid}/{link}")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string password, string passwordAgain, string adminResPasId, string link)
        {
            AdminResetPassword respas = _uow.AdminResPas.GetById(Convert.ToInt32(adminResPasId));
            respas.ResetDate = DateTime.Now;
            respas.Enabled = false;
            Admin admin = _uow.Admin.GetById(Convert.ToInt32(respas.AdminId));
            string resetLink = Cipher.Encrypt(respas.SendDate.ToString(), admin.Email);
            if (password == passwordAgain && resetLink.Replace("/", "-").Replace("+", "-") == link)
            {
                admin.Password = Cipher.Encrypt(password, admin.UserName);
                _uow.Admin.Update(admin);
                _uow.AdminResPas.Update(respas);
            }
            return Redirect("/admin/home/login");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult ClickData(string permalink, string leavinginpage)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1);
            DateTime dateTime = DateTime.Now;
            string userIp = _uow.PageAnalysis.GetUserIp;

            if (leavinginpage == "true")
            {
                PageAnalysis pageAnalysis = _uow.Cookie.GetPageAnalysisId();
                if (pageAnalysis.EndDate == null)
                {
                    pageAnalysis.EndDate = dateTime;
                    _uow.PageAnalysis.Update(pageAnalysis);
                }
                return Json(pageAnalysis.EndDate);
            }
            else
            {
                var userAgent = HttpContext.Request.Headers["User-Agent"];
                PageAnalysis pa = new PageAnalysis
                {
                    Ip = userIp,
                    EntryDate = DateTime.Now,
                    Page = permalink,
                    Lang = _uow.Cookie.GetWwwLangByCookie(),
                    Browser = userAgent
                };
                string encrypted = Cipher.Encrypt("SayfaAnaliz", "PageAnalysis");
                var analysisIdCookie = _uow.Cookie.GetCookie(encrypted);
                if (analysisIdCookie == null)
                {
                    _uow.Cookie.AddPAWithCookie(pa, encrypted, option);
                }
                else
                {
                    string analysisId = Cipher.Decrypt(analysisIdCookie, "PageAnalysis");
                    var pagean = _uow.PageAnalysis.GetById(Convert.ToInt32(analysisId));
                    if (pagean != null)
                    {
                        if ((DateTime.Now - pagean.EntryDate).TotalMinutes > 2)
                        {
                            _uow.Cookie.AddPAWithCookie(pa, encrypted, option);
                        }
                    }
                    else
                    {
                        _uow.Cookie.AddPAWithCookie(pa, encrypted, option);
                    }
                }
                return Json("");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/confirmation/{confirmid}/{confirmLink}")]
        public IActionResult ConfirmSubscription(string confirmid, string confirmLink)
        {
            ConfirmationEmail confirmationEmail = _uow.ConfirmationEmail.GetById(Convert.ToInt32(confirmid));

            string readLink = Cipher.Encrypt("confirmation", confirmationEmail.AdminEmail).Replace("/", "-").Replace("+", "-");

            if (confirmationEmail != null && confirmLink == readLink)
            {
                if (confirmationEmail.ReadDate == null)
                {
                    string name = _uow.ConfirmationEmail.UpdateUserStatus(confirmationEmail);

                    ViewData["AlertMessage"] = "Kaydınız onaylanmıştır. Sisteme bilgilerinizle giriş yapabilirsiniz. İlgin için teşekkürler <b>" + name + "</b>!";
                    return View("Login");
                }
                else
                {
                    ViewData["AlertMessage"] = "Kaydınız daha önceden onaylanmıştır. Sisteme bilgilerinizle giriş yapabilirsiniz.";
                    return View("Login");
                }

            }
            ViewData["ErrorMessage"] = "Kaydınız onaylanamamıştır. Lütfen site sahibi ile iletişime geçin. @ofcskn.";
            return View("Login");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Report()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Report(Contact entity)
        {
            if (ModelState.IsValid)
            {
                entity.Date = DateTime.Now;
                entity.Report = true;
                entity.Ip = _uow.Admin.GetIp();
                _uow.Contact.Add(entity);
                ViewBag.SuccessMessage = "Mesajınız bize ulaşmıştır. Teşekkür ederiz.";
                return RedirectToAction("Report", null);
            }
            ViewBag.ErrorMessage = "Lütfen gerekli alanları doğru şekilde doldurun.";
            return View(entity);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Rules()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
