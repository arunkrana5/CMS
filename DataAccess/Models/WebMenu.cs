using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.Models.AllEnum;

namespace DataAccess.Models
{
    public class WebMenu
    {
      
        public class List
        {
            public long WebMenuID { get; set; }
            public long DocID { get; set; }
            public string DocURL { get; set; }
            public string MenuType { get; set; }
            public string MenuName { get; set; }
            public string URLType { get; set; }
            public int ParentMenuID { get; set; }
            public string ParentMenuMap { get; set; }
            public string MenuURL { get; set; }
            public string Target { get; set; }
            public int? Priority { get; set; }
            public string IsChild { get; set; }
            public List<List> ChildMenuList { get; set; }
        }
        public class Admin
        {
            public class List
            {
                public long WebMenuID { get; set; }
                public long DocID { get; set; }
                public string DocURL { get; set; }
                public string MenuType { get; set; }
                public string MenuName { get; set; }
                public string URLType { get; set; }
                public int ParentMenuID { get; set; }
                public string ParentMenuMap { get; set; }
                public string MenuURL { get; set; }
                public string Target { get; set; }
                public int? Priority { get; set; }
                public string createdby { get; set; }
                public string createdat { get; set; }
                public string modifiedby { get; set; }
                public string modifiedat { get; set; }
                public bool IsActive { get; set; }
                public string IPAddress { get; set; }
                public string IsChild { get; set; }

            }
            public class Add
            {
                public long WebMenuID { get; set; }
                public long DocID { get; set; }
                public string DocURL { get; set; }
                public string MenuImage { get; set; }

                [Required(ErrorMessage = "Type Can't Be Blank")]
                public string MenuType { get; set; }
                [Required(ErrorMessage = "Name Can't Be Blank")]
                public string MenuName { get; set; }

                [Required(ErrorMessage = "Menu URL Type Can't Be Blank")]
                public string URLType { get; set; }

                public int ParentMenuID { get; set; }
                public string ParentMenuMap { get; set; }
                [Required(ErrorMessage = "URL Can't Be Blank")]
                public string MenuURL { get; set; }

                [Required(ErrorMessage = "Target Can't Be Blank")]
                public Target? Target { get; set; }

                [Required(ErrorMessage = "Priority Can't Be Blank")]
                public int? Priority { get; set; }
                public string IsChild { get; set; }
                public bool IsActive { get; set; }
                public long LoginID { get; set; }
                public string IPAddress { get; set; }
            }


            public class MenuURLType
            {
            
                public int ID { get; set; }
                public string Name { get; set; }
                public string URL { get; set; }
            }

            public class ParentChildCollection
            {
                public long WebMenuID { get; set; }
                public string MenuName { get; set; }
                public string IsChild { get; set; }
                public string ParentMenuMap { get; set; }
                public long ParentMenuID { get; set; }
                public string ParentMenuName { get; set; }

                public List<ParentChildCollection> ChildList { get; set; }

            }
        }
    }
}
