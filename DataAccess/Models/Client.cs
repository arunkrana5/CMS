using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
  public  class Client
    {
        public class Web
        {
            public long ClientID { get; set; }
            public string ClientCode { get; set; }
            public string FullName { get; set; }
            public long DocID { get; set; }
            public string ImageUrl { get; set; }
        }
        public class Admin
        {
            public class List
            {
                public long ClientID { get; set; }
                public string ClientCode { get; set; }
                public string FullName { get; set; }
                public string Address { get; set; }
                public long? CountryID { get; set; }
                public long? StateID { get; set; }
                public long? CityID { get; set; }
                public string CountryName { get; set; }
                public string StateName { get; set; }
                public string CityName { get; set; }
                public string OfficePhone { get; set; }
                public string MobileNo { get; set; }
                public string GSTNo { get; set; }
                public int Priority { get; set; }
                public string createdby { get; set; }
                public string createdat { get; set; }
                public string modifiedby { get; set; }
                public string modifiedat { get; set; }
                public bool IsActive { get; set; }
                public string IPAddress { get; set; }

                public long DocID { get; set; }
                public string DocURL { get; set; }
            }
            public class Add
            {
                public long ClientID { get; set; }
                public string ClientCode { get; set; }
                [Required(ErrorMessage = "Full Name Can't Be Blank")]
                public string FullName { get; set; }
                
                public string Address { get; set; }
                public long? CountryID { get; set; }
                public long? StateID { get; set; }
                public long? CityID { get; set; }
                public long DocID { get; set; }
                public string DocURL { get; set; }
                public string ImageUrl { get; set; }
                public string OfficePhone { get; set; }
                
                public string MobileNo { get; set; }
                public string GSTNo { get; set; }
                public int? Priority { get; set; }
                public bool IsActive { get; set; }
                public long LoginID { get; set; }
                public string IPAddress { get; set; }
            }
        }
    }
}
