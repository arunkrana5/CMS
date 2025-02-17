using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.Models.AllEnum;

namespace DataAccess.Models
{
    public class Banner
    {
        public class Web
        {
            public long BannerID { get; set; }
          
            public string BannerType { get; set; }
            public long DocID { get; set; }
            public string BannerURL { get; set; }
          
            public string Target { get; set; }
            public string Heading { get; set; }
            public string SubHeading { get; set; }
            public string InnerPageURL { get; set; }
            public int Priority { get; set; }
            public string GoToURL { get; set; }
        }

        public class Admin
        {
            public class List
            {
                public long BannerID { get; set; }
              
                public string BannerType { get; set; }
                public long DocID { get; set; }
                public string DocURL { get; set; }
                public string BannerURL { get; set; }
       
                public string Target { get; set; }
                public string Heading { get; set; }
                public string SubHeading { get; set; }
                public string InnerPageURL { get; set; }
                public int Priority { get; set; }
                public string GoToURL { get; set; }

                public string createdby { get; set; }
                public string createdat { get; set; }
                public string modifiedby { get; set; }
                public string modifiedat { get; set; }
                public bool IsActive { get; set; }
                public string IPAddress { get; set; }
            }
            public class Add
            {
                public long BannerID { get; set; }
                [Required(ErrorMessage = "Banner Type Can't Be Blank")]
                public string BannerType { get; set; }
                public long DocID { get; set; }
                public string DocURL { get; set; }
                public string BannerURL { get; set; }
                [Required(ErrorMessage = "Target Can't Be Blank")]
                public Target? Target { get; set; }

                public string Heading { get; set; }
                public string SubHeading { get; set; }
                public string InnerPageURL { get; set; }
                [Required(ErrorMessage = "Priority Can't Be Blank")]
                public int? Priority { get; set; }
                public string GoToURL { get; set; }
                public long LoginID { get; set; }
                public string IPAddress { get; set; }

            }
        }
    }
}
