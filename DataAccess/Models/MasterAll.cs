using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
  public  class MasterAll
    {

        public class Admin
        {
            public class List
            {
                public long ID { get; set; }
                public string table_name { get; set; }
                public string field_name { get; set; }
                public string field_value { get; set; }
                public long group_id { get; set; }
                public string group_Name { get; set; }
                public string group_Value { get; set; }
                public int srno { get; set; }
                public int Priority { get; set; }
                public string createdby { get; set; }
                public string createdat { get; set; }
                public string modifiedby { get; set; }
                public string modifiedat { get; set; }
                public bool IsActive { get; set; }
                public string IPAddress { get; set; }
                public bool Selection { get; set; }
            }
            public class Add
            {
                public long ID { get; set; }
                public string table_name { get; set; }
                [Required(ErrorMessage = "Name Can't be Blank")]
                public string field_name { get; set; }
                [Required(ErrorMessage = "Value Can't be Blank")]
                public string field_value { get; set; }
                [Required(ErrorMessage = "Can't be Blank")]
                public long group_id { get; set; }
                public int? Priority { get; set; }
                public bool IsActive { get; set; }
                public long LoginID { get; set; }
                public string IPAddress { get; set; }
            }
        }
    }
}
