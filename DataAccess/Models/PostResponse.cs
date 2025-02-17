using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class PostResponse
    {
        public string ViewAsString { get; set; }
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string SuccessMessage { get; set; }
        public string RedirectURL { get; set; }
        public long ID { get; set; }
        public long OtherID { get; set; }
        public string AdditionalMessage { get; set; }
    }
    public class FileResponse
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }
        public int FileLength { get; set; }
        public string ReadAbleFileSize { get; set; }
        public string FileExt { get; set; }
        public string FileType { get; set; }
        public System.IO.Stream InputStream { get; set; }
    }

    public class GetResponse
    {
        public long ID { get; set; }
        public long AdditionalID { get; set; }
        public string Doctype { get; set; }
        public long LoginID { get; set; }
        public string IPAddress { get; set; }

    }

    public class GetMasterResponse
    {
        public long ID { get; set; }
        public long GroupID { get; set; }
        public string TableName { get; set; }
        public string IsActive { get; set; }
        public long LoginID { get; set; }
        public string IPAddress { get; set; }

    }

    public class GetBannerResponse
    {
        public string BannerType { get; set; }
        public string InnerPageURL { get; set; }
        public long LoginID { get; set; }
        public string IPAddress { get; set; }

    }
    public class GetMenuResponse
    {
        public long WebMenuID { get; set; }
        public string MenuType { get; set; }
        public long ParentMenuID { get; set; }
        public string Doctype { get; set; }
        public long LoginID { get; set; }
        public string IPAddress { get; set; }

    }
    public class GetURLExitsResponse
    {
        public long ID { get; set; }
        public string URL { get; set; }
        public string Doctype { get; set; }
        public long LoginID { get; set; }
        public string IPAddress { get; set; }

    }

    public class GetUpdateColumnResponse
    {
        public long ID { get; set; }
        public string Value { get; set; }
        public string Doctype { get; set; }
        public long LoginID { get; set; }
        public string IPAddress { get; set; }

    }


    public class GetWebResponse
    {
        public long ID { get; set; }
        public long AdditionalID { get; set; }
        public string Doctype { get; set; }
        public string Parameter1 { get; set; }
        public string Parameter2 { get; set; }
        public string URL { get; set; }
        public string ShowAll { get; set; }
        public long LoginID { get; set; }
        public string IPAddress { get; set; }


    }

    public class GetURLExitsResponse_Web
    {
        
        public string Values { get; set; }
        public string Doctype { get; set; }
        public long LoginID { get; set; }
        public string IPAddress { get; set; }

    }
}

