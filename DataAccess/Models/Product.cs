using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Product
    {
       public class Web
        {
            public long ProductID { get; set; }
            public string ProductName { get; set; }
            public string ProductURL { get; set; }
            public string ProductHeading { get; set; }
            public string ProductShortDescription { get; set; }
            public string ProductDescription { get; set; }
            public string OtherDescription { get; set; }
            public string CategoryNames { get; set; }
            public decimal Amount { get; set; }
            public List<ProductImages.Web> ProductImagesList { get; set; }


        }
        public class Admin
        {
            public class List
            {
                public long ProductID { get; set; }
                public string ProductName { get; set; }
                public string ProductURL { get; set; }
                public string ProductHeading { get; set; }
                public string ProductShortDescription { get; set; }
                public string ProductDescription { get; set; }
                public string OtherDescription { get; set; }
                public bool IsActive { get; set; }
                public decimal Amount { get; set; }
                public int? Priority { get; set; }
                public string CategoryNames { get; set; }
                public string createdby { get; set; }
                public string createdat { get; set; }
                public string modifiedby { get; set; }
                public string modifiedat { get; set; }
                public string IPAddress { get; set; }
            }
            public class Add
            {
                public long ProductID { get; set; }
                [Required(ErrorMessage = "Name Can't Be Blank")]
                public string ProductName { get; set; }

                [Required(ErrorMessage = "URL Can't Be Blank")]
                public string ProductURL { get; set; }

                public string ProductHeading { get; set; }
                public string ProductShortDescription { get; set; }
                public string ProductDescription { get; set; }
                public string OtherDescription { get; set; }
                public decimal Amount { get; set; }
                public bool IsActive { get; set; }
                public long LoginID { get; set; }
                public int? Priority { get; set; }
                public string IPAddress { set; get; }
                [Required(ErrorMessage = "Categort Can't Be Blank")]
                public string CategoryIDs { get; set; }
                public List<DropdownList> CategoryList { get; set; }
            }
        }
    }
    public class ProductImages
    {
        public class Web
        {
            public long ProductImageID { get; set; }
            public long ProductID { get; set; }
            public string ImageName { get; set; }
            public string ImageType { get; set; }
            public string ImageDescription { get; set; }
            public string ImageURL { get; set; }
            public bool IsDefault { get; set; }
        }
        public class Admin
        {
            public class List
            {
                public long ProductImageID { get; set; }

                public long ProductID { get; set; }
              
                public string ImageName { get; set; }
                public string ImageType { get; set; }
                public long DocID { get; set; }
                public string DocURL { get; set; }
                public string ImageDescription { get; set; }
                public string ImageURL { get; set; }
                public bool IsDefault { get; set; }
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
                public long ProductImageID { get; set; }
                
                public long ProductID { get; set; }
                [Required(ErrorMessage = "Name Can't Be Blank")]
                public string ImageName { get; set; }
                [Required(ErrorMessage = "Type Can't Be Blank")]
                public string ImageType { get; set; }
                public long DocID { get; set; }
                public string DocURL { get; set; }

                public string ImageDescription { get; set; }
                public string ImageURL { get; set; }
                public bool IsDefault { get; set; }

                [Required(ErrorMessage = "Priority Can't Be Blank")]
                public int? Priority { get; set; }
                public long LoginID { get; set; }
                public string IPAddress { get; set; }
            }
        }
    }
    public class Category
    {
        public class Web
        {
            public long CategoryID { get; set; }
            public string CategoryName { get; set; }
            public string CategoryURL { get; set; }
            public string ImageURL { get; set; }
            public string Location { get; set; }
            public long Cat_DocID { get; set; }
            public string CategoryDescription { get; set; }
        }
        public class Admin
        {
            public class List
            {
                public long CategoryID { get; set; }
                public string CategoryName { get; set; }
                public string CategoryDescription { get; set; }
                public string CategoryURL { get; set; }
                public string ImageURL { get; set; }
                public long Cat_DocID { get; set; }
                public string Cat_DocURL { get; set; }
                public string Location { get; set; }
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
                public long CategoryID { get; set; }
                [Required(ErrorMessage = "Category Name Can't Be Blank")]
                public string CategoryName { get; set; }

                [Required(ErrorMessage = "URL Can't Be Blank")]
                public string CategoryURL { get; set; }
                public long Cat_DocID { get; set; }
                public string DocPath { get; set; }
                public string ImageURL { get; set; }
                public string CategoryDescription { get; set; }
                public string Location { get; set; }
                public bool IsActive { set; get; }
                public int? Priority { set; get; }
                public long LoginID { get; set; }
                public string IPAddress { set; get; }
            }
        }
    }


    public class ProductDemo
    {
        public long? PDID { get; set; }
        public long ProductID { get; set; }
        [Required(ErrorMessage = "Name Can't Be Blank")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email Can't Be Blank")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Can't Be Blank")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Message Can't Be Blank")]
        public string Message { get; set; }
        public long LoginID { get; set; }
        public string IPAddress { get; set; }

    }
}
