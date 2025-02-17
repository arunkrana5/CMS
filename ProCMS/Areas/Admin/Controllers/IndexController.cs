using DataAccess.Models;
using DataAccess.Setting;
using ProCMS.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
namespace ProCMS.Areas.Admin.Controllers
{
    [LoginFilter]
    public class IndexController : Controller
    {
        long LoginID = 0;
        string IPAddress = "";
        GetResponse getResponse;
        public IndexController()
        {
            getResponse = new GetResponse();
            long.TryParse(clsApplicationSetting.GetSessionValue("LoginID"), out LoginID);
            IPAddress = clsApplicationSetting.GetIPAddress();
            getResponse.IPAddress = IPAddress;
            getResponse.LoginID = LoginID;
        }

        string DesignKey = clsApplicationSetting.GetWebConfigValue("DesignKey");
        public ActionResult Dashboard()
        {
            //ClsCommon.CreateMenuJSon();
            //ClsCommon.GetMenuJSON();
            return View("Dashboard" + DesignKey);
        }

        [HttpPost]
        public ActionResult _DocumentList(string DocTypeID)
        {
            long ID = 0;
            long.TryParse(DocTypeID, out ID);
            List<Document.List> Modal = new List<Document.List>();
            getResponse.AdditionalID = ID;
            Modal = Common_SPU.GetDocumentList(getResponse);
            return PartialView(Modal);

        }
        public ActionResult _AddDocument()
        {
            Document.Add Modal = new Document.Add();
            return PartialView(Modal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult _AddDocument(Document.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            string OurDirectory = clsApplicationSetting.GetPhysicalPath("");
            Result.SuccessMessage = "Document is not uploaded";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;

                var DocType_Det = clsApplicationSetting.GetDocTypeList().Where(x => x.DocTypeID == Modal.DocTypeID).FirstOrDefault();
                OurDirectory = OurDirectory + "\\" + DocType_Det.DocTypeName;

                if (Modal.UploadDoc != null)
                {
                    foreach (HttpPostedFileBase Mulfile in Modal.UploadDoc)
                    {
                        var RvFile = clsApplicationSetting.ValidateFile(Mulfile);
                        if (RvFile.IsValid)
                        {
                            Modal.content_type = RvFile.FileExt;
                            Modal.Name = RvFile.FileName;
                            string FilePath = "";
                            Result = Common_SPU.fnSetDocument(Modal);
                            FilePath = OurDirectory + "/" + Result.ID.ToString() + RvFile.FileExt;
                            if (!Directory.Exists(OurDirectory))
                            {
                                Directory.CreateDirectory(OurDirectory);
                            }
                            if (Directory.Exists(OurDirectory))
                            {
                                if(System.IO.File.Exists(FilePath))
                                {
                                    System.IO.File.Delete(FilePath);
                                }
                                Mulfile.SaveAs(FilePath);
                            }
                            Result.OtherID = Modal.DocTypeID;
                        }
                        else
                        {
                            Result.Status = false;
                            Result.SuccessMessage = RvFile.Message;
                            break;
                        }
                    }
                }
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
    }
}