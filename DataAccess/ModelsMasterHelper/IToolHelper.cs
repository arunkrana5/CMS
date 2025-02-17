using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ModelsMasterHelper
{
    public interface IToolHelper
    {
        List<AdminMenu> GetAdminMenuList(GetResponse modal);
        List<ErrorLog> ErrorLogList(GetResponse modal);
        List<AdminUser.Role.List> GetRolesList(GetResponse modal);
        AdminUser.Role.Add GetRoles(GetResponse modal);
        List<AdminModule> GetModuleListWithMenu(GetResponse modal);
        PostResponse fnSetUserRole(AdminUser.Role.Add modal);
        List<EmailTemplate.List> GetEmailTemplateList(GetResponse modal);
        EmailTemplate.Add GetEmailTemplate(GetResponse modal);
        PostResponse fnSetEmailTemplate(EmailTemplate.Add modal);
        List<Users.List> GetUsersList(GetResponse modal);
        Users.Add GetUsers(GetResponse modal);
        PostResponse fnSetUsers(Users.Add modal);
    }
}
