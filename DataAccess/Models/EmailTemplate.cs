using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
  public  class EmailTemplate
    {
        public class List
        {
            public int RowNum { get; set; }
            public int ID { set; get; }

            [Required(ErrorMessage = "Template Name Can't be Blank")]
            public string TemplateName { get; set; }
            public string Body { get; set; }
            [Required(ErrorMessage = "Subject Can't be Blank")]
            public string Subject { set; get; }
            public string SMSBody { get; set; }
            public string Repository { get; set; }
            public string CCMail { get; set; }
            public string BCCMail { get; set; }
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
            public int ID { set; get; }

            [Required(ErrorMessage = "Template Name Can't be Blank")]
            public string TemplateName { get; set; }
            public string Body { get; set; }
            [Required(ErrorMessage = "Subject Can't be Blank")]
            public string Subject { set; get; }
            public string SMSBody { get; set; }
            public string Repository { get; set; }
            public string CCMail { get; set; }
            public string BCCMail { get; set; }
            public bool IsActive { set; get; }
            public int Priority { set; get; }
            public string IPAddress { get; set; }
            public long LoginID { get; set; }
        }
    }
}
