using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataAccess.Models
{
    public class Document
    {
        public class List
        {
            public int DocTypeID { get; set; }
            public string DocTypeName { get; set; }
            public string DisplayName { get; set; }

            public long DocID { get; set; }
            public string DocURL { get; set; }
            public string Name { get; set; }
            public string content_type { get; set; }

        }
        public class Add
        {
            public long DocID { get; set; }
            [Required(ErrorMessage = "Document Type Can't Be Blank")]
            public int DocTypeID { get; set; }
            public HttpPostedFileBase[] UploadDoc { get; set; }

            public string Name { get; set; }
            public string content_type { get; set; }

            public long LoginID { get; set; }
            public string IPAddress { get; set; }

        }
    }
    public class DocumentType
    {
        public class List
        {
            public long DocTypeID { get; set; }
            public string DocTypeName { get; set; }
            public string DisplayName { get; set; }
           
            public string createdby { get; set; }
            public string createdat { get; set; }
            public string modifiedby { get; set; }
            public string modifiedat { get; set; }
           public int Priority { get; set; }
            public bool IsActive { get; set; }
            public string IPAddress { get; set; }
        }
        public class Add
        {
            public long DocTypeID { get; set; }
            [Required(ErrorMessage = "Type Name Can't Be Blank")]
            public string DocTypeName { get; set; }
            [Required(ErrorMessage = "Display Name Can't Be Blank")]
            public string DisplayName { get; set; }
           
      
            public int IsActive { get; set; }
            public int? Priority { get; set; }
            public long LoginID { get; set; }
            public string IPAddress { get; set; }
        }
    }
}
