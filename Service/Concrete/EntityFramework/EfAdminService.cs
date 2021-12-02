using Entity.Context;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Service.Abstract;
using Service.Utilities;

namespace Service.Concrete.EntityFramework
{
    public class EfAdminService : EfGenericService<Admin>, IAdminService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EfAdminService(SwappDbContext _context, IHttpContextAccessor httpContextAccessor) : base(_context)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }

        public string GetIp()
        {
            return _httpContextAccessor.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
        }
        public string GetNameByAdmin
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "name").Value; }
        }
        public string GetImageByAdmin
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "image").Value; }
        }

        public string GetRoleByAdmin
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "role").Value; }
        }

        public string GetLastLoginDate
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "lastLoginDate").Value; }
        }
        public string GetLastLoginIp
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "lastLoginIp").Value; }
        }

        public string GetUserName
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "username").Value; }
        }

        public string GetMailByAdmin
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "mail").Value; }
        }

        public int GetIdByAdmin
        {
            get { return Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "id").Value); }
        }

        public Admin IsAdmin(int id)
        {
            var all = _db.Admin.FirstOrDefault(p => p.IsAdmin == true);
            all.IsAdmin = false;
            var admin = GetById(id);
            admin.IsAdmin = true;
            Update(admin);
            return admin;
        }
        public Admin GetAdmin(string userName, string password)
        {
            var encryptedText = Cipher.Encrypt(password, userName);
            return _db.Admin.FirstOrDefault(p => p.UserName == userName && p.Password == encryptedText);
        }
        public bool ControlUserName(string userName, string role, int id)
        {
            if (_db.Admin.Any(p => p.UserName == userName.ToLower()) || _db.Current.Any(p => p.UserName == userName.ToLower()) || _db.Staff.Any(p => p.UserName == userName.ToLower()))
            {
                if (role == "admin" || role == "user")
                {
                    if (_db.Admin.FirstOrDefault(p => p.Id == id).UserName == userName)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (role == "current")
                {
                    if (_db.Current.Any(p => p.UserName == userName.ToLower()))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (role == "staff")
                {
                    if (_db.Staff.FirstOrDefault(p => p.Id == id).UserName == userName)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        public bool ControlUserEmail(string Email, string role, int id)
        {
            if (_db.Admin.Any(p => p.Email == Email.ToLower()) || _db.Current.Any(p => p.Mail == Email.ToLower()) || _db.Staff.Any(p => p.Email == Email.ToLower()))
            {
                if (role == "admin" || role == "user")
                {
                    if (_db.Admin.FirstOrDefault(p => p.Id == id).Email == Email)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (role == "current")
                {
                    if (_db.Current.Any(p => p.Mail == Email.ToLower()))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (role == "staff")
                {
                    if (_db.Staff.FirstOrDefault(p => p.Id == id).Email == Email)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        public string GetUserNameSurname(string role, int id, string filter)
        {
            string name = null;
            string surname = null;
            string username = null;
            if (role == "admin" || role == "user")
            {
                var admin = _db.Admin.FirstOrDefault(p => p.Id == id);
                if (admin != null)
                {
                    name = admin.Name;
                    surname = admin.Surname;
                    username = admin.UserName;
                }
            }
            else if (role == "current")
            {
                var current = _db.Current.FirstOrDefault(p => p.Id == id);
                if (current != null)
                {
                    name = current.Name;
                    surname = current.Surname;
                    username = current.UserName;
                }
            }
            else if (role == "staff")
            {
                var staff = _db.Staff.FirstOrDefault(p => p.Id == id);
                if (staff != null)
                {
                    name = staff.Name;
                    surname = staff.Surname;
                    username = staff.UserName;
                }
            }
            if (filter == "name")
            {
                return name + " " + surname;
            }
            else
            {
                return username;
            }
        }
        public string GetUserNameByEmail(string email)
        {
            if (_db.Admin.Any(p => p.Email == email))
            {
                return _db.Admin.FirstOrDefault(p => p.Email == email).UserName;
            }
            else if (_db.Current.Any(p => p.Mail == email))
            {
                return _db.Current.FirstOrDefault(p => p.Mail == email).UserName;
            }
            else if (_db.Staff.Any(p => p.Email == email))
            {
                return _db.Staff.FirstOrDefault(p => p.Email == email).UserName;
            }
            else
            {
                return null;
            }
        }
        public string GetEmailByIdByRole(string role, int id)
        {
            string email = null;
            if (role == "admin" || role == "user")
            {
                var admin = _db.Admin.FirstOrDefault(p => p.Id == id);
                if (admin != null)
                {
                    email = admin.Email;
                }
            }
            else if (role == "current")
            {
                var current = _db.Current.FirstOrDefault(p => p.Id == id);
                if (current != null)
                {
                    email = current.Mail;
                }
            }
            else if (role == "staff")
            {
                var staff = _db.Staff.FirstOrDefault(p => p.Id == id);
                if (staff != null)
                {
                    email = staff.Email;
                }
            }
            return email;
        }
        public string GetUserImage(string role, int id)
        {
            string email = null;
            if (role == "admin" || role == "user")
            {
                var admin = _db.Admin.FirstOrDefault(p => p.Id == id);
                if (admin != null)
                {
                    email = admin.Image == null ? "default.jpg" : _db.Admin.FirstOrDefault(p => p.Id == id).Image;
                }
            }
            else if (role == "current")
            {
                var current = _db.Current.FirstOrDefault(p => p.Id == id);
                if (current != null)
                {
                    email = current.Image == null ? "default.jpg" : current.Image;
                }
            }
            else if (role == "staff")
            {
                var staff = _db.Staff.FirstOrDefault(p => p.Id == id);
                if (staff != null)
                {
                    email = staff.Image == null ? "default.jpg" : staff.Image;
                }
            }
            return email;
        }
        public Admin GetAdminByRememberForm(string email, int pinCode)
        {
            return _db.Admin.FirstOrDefault(p => p.PinCode == pinCode && p.Email == email);
        }

        public IQueryable<Admin> GetAllByEnabled(bool status)
        {
            return _db.Admin.Where(p => p.Enabled == status);
        }
        public bool ControlUserNameForAll(string userName)
        {
            if (_db.Admin.Any(p => p.UserName == userName) || _db.Current.Any(p => p.UserName == userName) || _db.Staff.Any(p => p.UserName == userName))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool ControlUserEmailForAll(string email)
        {
            if (_db.Admin.Any(p => p.Email == email) || _db.Current.Any(p => p.Mail == email) || _db.Staff.Any(p => p.Email == email))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
