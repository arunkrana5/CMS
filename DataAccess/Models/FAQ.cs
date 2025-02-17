using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
   public class FAQ
    {
        public class Web
        {
            public long FAQID { get; set; }
            public string Question { get; set; }
            public string Answer { get; set; }
            public string FAQURL { set; get; }
        }
        public class List
        {
            public long FAQID { get; set; }
            public string Question { get; set; }
            public string Answer { get; set; }
            public string FAQURL { set; get; }
            public bool IsActive { set; get; }
            public int Priority { set; get; }
        }
        public class Admin
        {
            public class List
            {
                public long FAQID { get; set; }
                public string Question { get; set; }
                public string Answer { get; set; }
                public string FAQURL { set; get; }
                public bool IsActive { set; get; }
                public int Priority { set; get; }
                public string createdby { set; get; }
                public string createdat { set; get; }
                public string modifiedby { set; get; }
                public string modifiedat { set; get; }
                public string IPAddress { set; get; }
            }
            public class Add
            {
                public long FAQID { get; set; }
                [Required(ErrorMessage = "Question Can't be Blank")]
                public string Question { get; set; }
                public string Answer { get; set; }

                [StringLength(60, ErrorMessage = "Allowed Maximum 60 Characters")]
                [Required(ErrorMessage = "URL can't be blank")]
                public string FAQURL { set; get; }
                public bool IsActive { set; get; }
                public int? Priority { set; get; }
                public long LoginID { get; set; }
                public string IPAddress { set; get; }
            }
        }
    }
}
