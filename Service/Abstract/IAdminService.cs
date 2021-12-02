using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IAdminService : IGenericService<Admin>
    {
        Admin IsAdmin(int id);
        Admin GetAdmin(string username, string password);
        IQueryable<Admin> GetAllByEnabled(bool status);
        string GetIp();
        string GetNameByAdmin { get; }
        string GetImageByAdmin { get; }
        string GetUserName { get; }
        string GetRoleByAdmin { get; }
        int GetIdByAdmin { get; }
        string GetLastLoginDate { get; }
        string GetLastLoginIp { get; }
        Admin GetAdminByRememberForm(string email, int pinCode);
        string GetMailByAdmin { get; }
        bool ControlUserName(string userName, string role, int id);
        bool ControlUserEmail(string email ,string role, int id);
        string GetUserNameSurname(string role, int id, string filter);
        string GetUserImage(string role, int id);
        string GetEmailByIdByRole(string role, int id);
        string GetUserNameByEmail(string email);
        bool ControlUserNameForAll(string userName);
        bool ControlUserEmailForAll(string email);
    }
}
