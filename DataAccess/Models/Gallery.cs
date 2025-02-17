using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
   public class Gallery
    {
        public class Web
        {
            public long RowNum { get; set; }
            public long GalleryID { get; set; }
            public long GalleryGroupID { get; set; }
            public string GroupHeading { get; set; }
            public string GroupName { get; set; }
            public long DocID { get; set; }
            public string DocURL { get; set; }
            public string Heading { get; set; }
            public string Description { get; set; }

            public string ImageURL { get; set; }
        }
        public class Admin
        {
            public class List
            {
                public long RowNum { get; set; }
                public long GalleryID { get; set; }
                public long GalleryGroupID { get; set; }
                public string GroupName { get; set; }
                public long DocID { get; set; }
                public string DocURL { get; set; }
                public string Heading { get; set; }
                public string Description { get; set; }
       
                public string ImageURL { get; set; }
                public int Priority { get; set; }
                public string createdby { get; set; }
                public string createdat { get; set; }
                public string modifiedby { get; set; }
                public string modifiedat { get; set; }

                public bool IsActive { get; set; }

                public string IPAddress { get; set; }
            }
            public class Add
            {
                public long GalleryID { get; set; }
                [Required(ErrorMessage = "Group Can't Be Blank")]
                public long GalleryGroupID { get; set; }
                public string GroupName { get; set; }
                public long DocID { get; set; }
                public string DocURL { get; set; }
                [Required(ErrorMessage = "Heading Can't Be Blank")]
                public string Heading { get; set; }
                public string Description { get; set; }

                public string ImageURL { get; set; }
                public int? Priority { get; set; }
                public long LoginID { get; set; }
                public string IPAddress { get; set; }
            }
        }
    }

    public class GalleryGroup
    {
        public class Admin
        {
            public class List
            {
                public long GalleryGroupID { get; set; }
                public string GroupName { get; set; }
                public string Description { get; set; }
                public long GroupDocID { get; set; }
                public string GroupDocURL { get; set; }
                public string ImageURL { get; set; }
                public int Priority { get; set; }
                public string createdby { get; set; }
                public string createdat { get; set; }
                public string modifiedby { get; set; }
                public string modifiedat { get; set; }
         
                public bool IsActive { get; set; }
                
                public string IPAddress { get; set; }
            }
            public class Add
            {
                public long GalleryGroupID { get; set; }
                [Required(ErrorMessage = "Group Name Can't Be Blank")]
                public string GroupName { get; set; }
                public string Description { get; set; }
                public long GroupDocID { get; set; }
                public string GroupDocURL { get; set; }
                public string ImageURL { get; set; }
                [Required(ErrorMessage = "Priority Can't Be Blank")]
                public int? Priority { get; set; }
                public long LoginID { get; set; }
                public string IPAddress { get; set; }
            }
        }
    }
}
