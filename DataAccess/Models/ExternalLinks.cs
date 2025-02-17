using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
 public   class ExternalLinks
    {
        public class Web
        {
            public long LinkID { get; set; }
            public string LinkName { get; set; }
            public string LinkURL { get; set; }
            public long DocID { get; set; }
            public string ImageURL { get; set; }
        }
        public class List
        {
            public long LinkID { get; set; }
            public string LinkName { get; set; }
            public string LinkURL { get; set; }
            public long DocID { get; set; }
            public string ImageURL { get; set; }
            public int Priority { get; set; }
            public int ShowInHome { get; set; }
        }
        public class Admin
        {
            public class List
            {
                public long LinkID { get; set; }
                public string LinkName { get; set; }
               
                public string LinkURL { get; set; }
                public long DocID { get; set; }
                public string DocURL { get; set; }
                public string ImageURL { get; set; }
                public int Priority { get; set; }
                public string createdby { get; set; }
                public string createdat { get; set; }
                public string modifiedby { get; set; }
                public string modifiedat { get; set; }
                public bool IsActive { get; set; }
                public bool ShowInHome { get; set; }
                public string IPAddress { get; set; }
            }
            public class Add
            {
                public long LinkID { get; set; }
                [Required(ErrorMessage = "Link Name Can't Be Blank")]
                public string LinkName { get; set; }
                [Required(ErrorMessage = "Link URL Can't Be Blank")]
                public string LinkURL { get; set; }
                public long DocID { get; set; }
                public string DocURL { get; set; }
                public string ImageURL { get; set; }
                public int? Priority { get; set; }
                public string IPAddress { get; set; }
                public bool IsActive { get; set; }
                public long LoginID { get; set; }
            }
        }
    }
}
