using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
   public class Testimonials
    {
        public class List
        {
            public long TestimonialID { get; set; }
            public long DocID { get; set; }
            public string DocURL { get; set; }
            public string Heading { get; set; }
            public string Description { get; set; }
            public string Author { get; set; }
            public string ImageURL { get; set; }

            public string VideoURL { get; set; }
            public int Priority { get; set; }
        }
        public class Admin
        {
            public class List
            {
                public long TestimonialID { get; set; }
                public long DocID { get; set; }
                public string DocURL { get; set; }
                public string Heading { get; set; }
                public string Description { get; set; }
                public string Author { get; set; }
                public string ImageURL { get; set; }
              
                public string VideoURL { get; set; }
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
                public long TestimonialID { get; set; }
                public long DocID { get; set; }
                public string DocURL { get; set; }
                [Required(ErrorMessage = "Heading Can't Be Blank")]
                public string Heading { get; set; }
                public string Description { get; set; }
                [Required(ErrorMessage = "Author Can't Be Blank")]
                public string Author { get; set; }
                public string ImageURL { get; set; }
                public string VideoURL { get; set; }
                public int? Priority { get; set; }
                public long LoginID { get; set; }
                public string IPAddress { get; set; }
            }
        }
    }
}
