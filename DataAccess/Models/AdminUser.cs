using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class AdminUser
    {
        public class Login
        {

            [Required(ErrorMessage = "User Name Can't Be Blank")]
            public string UserName { get; set; }
            [Required(ErrorMessage = "Password Can't Be Blank")]
            public string Password { get; set; }

        }

        public class Role
        {
            public class List
            {
                public long ID { get; set; }
                public string role_name { get; set; }
                public string description { get; set; }
                public string createdat { get; set; }
                public string modifiedat { get; set; }
                public string IPAddress { get; set; }
                public bool IsActive { get; set; }
            }
            public class Add
            {
                public long ID { get; set; }
                [Required(ErrorMessage = "Role Name Can't Be Blank")]
                public string role_name { get; set; }
                public string description { get; set; }
                public string IPAddress { get; set; }
                public long LoginID { get; set; }
            }
        }
    }
    public class Users
    {
        public class List
        {
            public int RowNum { get; set; }
            public long ID { get; set; }
            public string UserID { get; set; }
            public int RoleID { get; set; }
            public string role_name { get; set; }
            public string Phone { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            public string Description { get; set; }
            public string email { get; set; }
            public int Priority { get; set; }
            public bool IsActive { get; set; }
            public string createdby { set; get; }
            public string createdat { set; get; }
            public string modifiedby { set; get; }
            public string modifiedat { set; get; }
            public string IPAddress { set; get; }
        }
        public class Add
        {
            public long ID { get; set; }
            [Required(ErrorMessage = "User ID Can't Be Blank")]
            public string UserID { get; set; }

            [Required(ErrorMessage = "Role Can't Be Blank")]
            public int RoleID { get; set; }
            public string RoleName { get; set; }
            public string Phone { get; set; }
            [Required(ErrorMessage = "Name Can't Be Blank")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Password Can't Be Blank")]
            public string Password { get; set; }
            public string Description { get; set; }

            [StringLength(200, ErrorMessage = "Allowed Maximum Length 200 Characters")]
            [DataType(DataType.EmailAddress)]
            [EmailAddress]
            public string email { get; set; }
            public int? Priority { get; set; }
            public long LoginID { get; set; }

            public string IPAddress { set; get; }
        }
    }
    public class DashboardItems
    {

    }
}
