using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class CMS
    {
        public class List
        {
            public long CMSID { get; set; }
            public string Heading { get; set; }
            public string PageURL { get; set; }
            public string createdby { get; set; }
            public string createdat { get; set; }
            public string modifiedby { get; set; }
            public string modifiedat { get; set; }
            public bool IsActive { get; set; }
            public string IPAddress { get; set; }
        }
        public class Add
        {
            public long CMSID { get; set; }
            [Required(ErrorMessage = "Heading Can't Be Blank")]
            public string Heading { get; set; }
            [Required(ErrorMessage = "Page URL Can't Be Blank")]
            public string PageURL { get; set; }
            public int? Priority { get; set; }
            public bool IsActive { get; set; }
            public long LoginID { get; set; }
            public string IPAddress { get; set; }
        }
    }


    public class CMSSection
    {
        public class List
        {
            public int CMSSectionID { get; set; }
            public int CMSID { get; set; }
            public string SectionName { get; set; }
            public string Content { get; set; }
            public int? Priority { get; set; }
            public string createdby { get; set; }
            public string createdat { get; set; }
            public string modifiedby { get; set; }
            public string modifiedat { get; set; }
            public bool IsActive { get; set; }
            public string IPAddress { get; set; }
        }
        public class Add
        {
            public long CMSSectionID { get; set; }
            public long CMSID { get; set; }
            [Required(ErrorMessage = "Section Name Can't Be Blank")]
            public string SectionName { get; set; }

            public string Content { get; set; }
            [Required(ErrorMessage = "Priority Can't Be Blank")]
            public int? Priority { get; set; }
            public bool IsActive { get; set; }
            public long LoginID { get; set; }
            public string IPAddress { get; set; }
        }
    }

    public class CMSContent
    {
        public int CMSID { get; set; }
        public int CMSSectionID { get; set; }
        public string Heading { get; set; }
        public string PageURL { get; set; }
        public string SectionName { get; set; }
        public string Content { get; set; }
        public int Priority { get; set; }

    }
}
