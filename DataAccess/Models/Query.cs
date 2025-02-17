using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Query
    {
        public class Add
        {
            public long? QueryID { get; set; }
            public string Type { get; set; }
            [Required(ErrorMessage = "Name Can't Be Blank")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Email Can't Be Blank")]
            [DataType(DataType.EmailAddress)]
            [EmailAddress]
            public string Email { get; set; }
            public string Subject { get; set; }
            [Required(ErrorMessage = "Mobile Can't Be Blank")]
            public string Mobile { get; set; }
            [Required(ErrorMessage = "Query Can't Be Blank")]
            public string Query { get; set; }

            public long LoginID { get; set; }
            public string IPAddress { get; set; }
        }
        public class List
        {
            public long QueryID { get; set; }
            public string Type { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Subject { get; set; }
            public string Mobile { get; set; }
            public string query { get; set; }
            public string createdby { get; set; }
            public string createdat { get; set; }
            public string modifiedby { get; set; }
            public string modifiedat { get; set; }
            public bool IsActive { get; set; }
            public string IPAddress { get; set; }
        }
    }
}
