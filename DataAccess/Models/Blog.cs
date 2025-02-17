using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.Models.AllEnum;

namespace DataAccess.Models
{
   public class Blog
    {
        public class Details
        {
            public long BlogID { get; set; }
            public long ListDocID { get; set; }
            public string ListDocURL { get; set; }
            public long DetailDocID { get; set; }
            public string DetailDocURL { get; set; }
            public string Heading { get; set; }
            public int BlogTypeID { get; set; }
            public string TypeName { get; set; }
            public string TypeURL { get; set; }
            public string BlogCategoryIDs { get; set; }
            public string BlogTagIDs { get; set; }
            public string BlogCategoryNames { get; set; }
            public string BlogTagNames { get; set; }
            public string BlogURL { get; set; }
            public string ListImageURL { get; set; }
            public string DetailImageURL { get; set; }
            public string ShortDesc { get; set; }
            public string Target { get; set; }
            public string AuthorName { get; set; }
            public DateTime PublishedDate { get; set; }
            public long CommentCount { get; set; }
            public long LikeCount { get; set; }
            public string Description { get; set; }

            public List<BlogRelatedInformation> AllCategory { get; set; }
            public List<BlogRelatedInformation> AllTags { get; set; }

            public List<Web> BlogList { get; set; }
            public List<ExternalLinks> ExternalLinksList { get; set; }
        }
        public class Web
        {
            public int RowNum { get; set; }
            public long BlogID { get; set; }
            public long ListDocID { get; set; }
            public string ListDocURL { get; set; }
            public long DetailDocID { get; set; }
            public string DetailDocURL { get; set; }
            public string Heading { get; set; }
            public int BlogTypeID { get; set; }
            public string TypeName { get; set; }
            public string TypeURL { get; set; }
            public string BlogCategoryIDs { get; set; }
            public string BlogTagIDs { get; set; }
            public string BlogCategoryNames { get; set; }
            public string BlogTagNames { get; set; }
            public string BlogURL { get; set; }
            public string ListImageURL { get; set; }
            public string DetailImageURL { get; set; }
            public string ShortDesc { get; set; }
            public string Target { get; set; }
            public string AuthorName { get; set; }
            public DateTime PublishedDate { get; set; }
            public int Priority { get; set; }
            public long CommentCount { get; set; }
            public long LikeCount { get; set; }
            public string Description { get; set; }
            public List<BlogRelatedInformation> AllCategory { get; set; }
            public List<BlogRelatedInformation> AllTags { get; set; }
        }
        public class Admin
        {
            public class List
            {
                public long BlogID { get; set; }
                public long ListDocID { get; set; }
                public string ListDocURL { get; set; }
                public long DetailDocID { get; set; }
                public string DetailDocURL { get; set; }
                public string Heading { get; set; }
                public int BlogTypeID { get; set; }
                public string TypeName { get; set; }
                public string BlogCategoryIDs { get; set; }
                public string BlogTagIDs { get; set; }
                public string BlogCategoryNames { get; set; }
                public string BlogTagNames { get; set; }
                public string BlogURL { get; set; }
                public string ListImageURL { get; set; }
                public string DetailImageURL { get; set; }
                public string ShortDesc { get; set; }
                public string Target { get; set; }
                public string AuthorName { get; set; }
                public string PublishedDate { get; set; }
                public int Priority { get; set; }
                public long CommentCount { get; set; }
                public long LikeCount { get; set; }
                public string Description { get; set; }
                public string createdby { get; set; }
                public string createdat { get; set; }
                public string modifiedby { get; set; }
                public string modifiedat { get; set; }
                public bool IsActive { get; set; }
                public bool ShowInHome { get; set; }
                public string IPAddress { get; set; }
            }
            public class Add
            {
                public long ListDocID { get; set; }
                public string ListDocURL { get; set; }
                public long DetailDocID { get; set; }
                public string DetailDocURL { get; set; }
                public long BlogID { get; set; }
                [Required(ErrorMessage = "Heading Can't Be Blank")]
                public string Heading { get; set; }
                public string BlogCategoryNames { get; set; }
                public string BlogTagNames { get; set; }
                [Required(ErrorMessage = "Choose Blog Type")]
                public int BlogTypeID { get; set; }
                public string TypeName { get; set; }

                [Required(ErrorMessage = "Choose Atleast one Category")]
                public string BlogCategoryIDs { get; set; }

                [Required(ErrorMessage = "Choose Atleast one Tag")]
                public string BlogTagIDs { get; set; }
                [Required(ErrorMessage = "Blog URL Can't Blank")]
                public string BlogURL { get; set; }
                public string ListImageURL { get; set; }
                public string DetailImageURL { get; set; }

                [Required(ErrorMessage = "Short Description Can't Blank")]
                public string ShortDesc { get; set; }

                public string Description { get; set; }
                [Required(ErrorMessage = "Target Can't Be Blank")]
                public Target? Target { get; set; }
                public string AuthorName { get; set; }
                public string PublishedDate { get; set; }
                public int? Priority { get; set; }
                public bool IsActive { get; set; }
                public long LoginID { get; set; }
                public string IPAddress { get; set; }
            }
        }
    }

    public class BlogType
    {
        public class Admin
        {
            public class List
            {
                public long BlogTypeID { get; set; }
                public string TypeName { get; set; }
                public string TypeURL { get; set; }
                public int Priority { get; set; }
                public string Description { get; set; }
                public int createdby { get; set; }
                public string createdat { get; set; }
                public int modifiedby { get; set; }
                public string modifiedat { get; set; }
                public bool IsActive { get; set; }
                public string IPAddress { get; set; }
            }
            public class Add
            {
                public long BlogTypeID { get; set; }

                [Required(ErrorMessage = "Blog Type Can't Blank")]
                public string TypeName { get; set; }

                [Required(ErrorMessage = "Blog Type URL Can't Blank")]
                public string TypeURL { get; set; }
                public int? Priority { get; set; }
                public string Description { get; set; }
                public bool IsActive { get; set; }
                public long LoginID { get; set; }
                public string IPAddress { get; set; }

            }
        }
    }

    public class BlogTags
    {
        public class Web
        {
            public long BlogTagID { get; set; }
            public string TagName { get; set; }
            public string TagURL { get; set; }
        }
        public class Admin
        {
            public class List
            {
                public long BlogTagID { get; set; }
                public string TagName { get; set; }
                public string TagURL { get; set; }
                public int Priority { get; set; }
                public string Description { get; set; }
                public string createdby { get; set; }
                public string createdat { get; set; }
                public string modifiedby { get; set; }
                public string modifiedat { get; set; }
                public bool IsActive { get; set; }
                public string IPAddress { get; set; }

            }
            public class Add
            {
                public long BlogTagID { get; set; }
                [Required(ErrorMessage = "Tag Name Can't Blank")]
                public string TagName { get; set; }
               
                [Required(ErrorMessage = "Tag URL Can't Blank")]
                public string TagURL { get; set; }
                public int? Priority { get; set; }
                public string Description { get; set; }
                public bool IsActive { get; set; }
                public long LoginID { get; set; }
                public string IPAddress { get; set; }
            }
        }
    }

    public class BlogCategory
    {
        public class Web
        {
            public long CategoryID { get; set; }
            public string CategoryName { get; set; }
            public string CategoryURL { get; set; }
            public string CategoryDocURL { get; set; }

        }
        public class Admin
        {
            public class List
            {
                public long BlogCategoryID { get; set; }
                [Required(ErrorMessage = "Category Name Can't Blank")]
                public string CategoryName { get; set; }
                [Required(ErrorMessage = "Category URL Can't Blank")]
                public string CategoryURL { get; set; }
                public string CategoryImage { get; set; }
                public string CategoryDesc { get; set; }
                public int Priority { get; set; }
                public long CategoryDocID { get; set; }
                public string CategoryDocURL { get; set; }
                public string createdby { get; set; }
                public string createdat { get; set; }
                public string modifiedby { get; set; }
                public string modifiedat { get; set; }
                public bool IsActive { get; set; }
                public string IPAddress { get; set; }
            }
            public class Add
            {
                public long BlogCategoryID { get; set; }
                [Required(ErrorMessage = "Category Name Can't Blank")]
                public string CategoryName { get; set; }
                [Required(ErrorMessage = "Category URL Can't Blank")]
                public string CategoryURL { get; set; }
                public string CategoryImage { get; set; }
                public string CategoryDesc { get; set; }

                public long CategoryDocID { get; set; }
                public string CategoryDocURL { get; set; }

                public int? Priority { get; set; }
                public bool IsActive { get; set; }
                public long LoginID { get; set; }
                public string IPAddress { get; set; }
            }
        }
    }


    public class BlogRelatedInformation
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string TypeURL { get; set; }
        public string ImageURL { get; set; }
    }

    public class Web_LikeComment
    {
        [StringLength(75, ErrorMessage = "Allowed Maximum 75 Characters")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Allowed only Alphabets")]
        [Required(ErrorMessage = "Name can't be blank")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [StringLength(100, ErrorMessage = "Allowed Maximum Length 100 Characters")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter valid email address")]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(10, ErrorMessage = "Allowed Maximum Length 10 Characters")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Allowed only Numbers")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Comment can't be blank")]
        public string Comment { get; set; }


    }
}
