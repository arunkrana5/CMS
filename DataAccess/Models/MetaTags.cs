using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
   public class MetaTags
    {
       public class Web
        {
            public string MetaName { get; set; }
            public string MetaValue { get; set; }
        }

        public class Admin
        {
            public class List
            {
                public long MetaTagsID { get; set; }
                [Required(ErrorMessage = "Page URL Can't Be Blank")]
                public string PageURL { get; set; }
                [Required(ErrorMessage = "Meta Name Can't Be Blank")]
                public string MetaName { get; set; }
                [Required(ErrorMessage = "Content Can't Be Blank")]
                public string MetaValue { get; set; }
                public int Priority { get; set; }
                public string createdby { get; set; }
                public string createdat { get; set; }
                public string modifiedby { get; set; }
                public string modifiedat { get; set; }
                public bool IsActive { get; set; }
                public bool IsDefault { get; set; }
                public string IPAddress { get; set; }
            }
            public class Add
            {
                public long MetaTagsID { get; set; }
                [Required(ErrorMessage = "Page URL Can't Be Blank")]
                public string PageURL { get; set; }
                [Required(ErrorMessage = "Meta Name Can't Be Blank")]
                public string MetaName { get; set; }
                [Required(ErrorMessage = "Content Can't Be Blank")]
                public string MetaValue { get; set; }
                public int? Priority { get; set; }
                public bool IsActive { get; set; }
                public bool IsDefault { get; set; }
                public long LoginID { get; set; }
                public string IPAddress { get; set; }
            }
        }
    }
}
