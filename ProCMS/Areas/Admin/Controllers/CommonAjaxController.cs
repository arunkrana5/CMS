using DataAccess.Models;
using DataAccess.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProCMS.Areas.Admin.Controllers
{
    public class CommonAjaxController : Controller
    {
        long LoginID = 0;
        string IPAddress = "";
        GetResponse getResponse;
        public CommonAjaxController()
        {
            getResponse = new GetResponse();
            long.TryParse(clsApplicationSetting.GetSessionValue("LoginID"), out LoginID);
            IPAddress = clsApplicationSetting.GetIPAddress();
            getResponse.IPAddress = IPAddress;
            getResponse.LoginID = LoginID;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult EncryptQueryStringJSON(string Value)
        {
            return Json(clsCommon.Encrypt(Value), JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult DecryptQueryStringJSON(string Value)
        {
            return Json(clsApplicationSetting.DecryptQueryString(Value), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetDateTimeJson()
        {
            string MyTime = DateTime.Now.ToString("dddd, dd-MMM-yyyy hh:mm:ss tt");
            return Json(MyTime, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult IsSessionEndJSON()
        {
            return Json(clsApplicationSetting.IsSessionExpired("LoginID"), JsonRequestBehavior.AllowGet);

        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetStrickyNotesJson()
        {
            string Notes = clsDataBaseHelper.ExecuteSingleResult("select StrickyNotes from users where ID=" + LoginID);
            return Json(Notes, JsonRequestBehavior.AllowGet);

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SetStrickyNotesJson(string StrickyNotes)
        {

            bool tre = false;
            try
            {

                clsDataBaseHelper.ExecuteNonQuery("update users Set StrickyNotes='" + clsCommon.EnsureString(StrickyNotes) + "' where ID=" + LoginID);
                tre = true;
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during SetUserStrickyNotesJson. The query was executed :", ex.ToString(), "", "CommonAjax", "CommonAjax", LoginID, IPAddress);
            }
            return Json(tre, JsonRequestBehavior.AllowGet);


        }


        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult DeleteDocumentJSON(string DocID)
        {
            bool Isreul = false;
            long ID = 0;
            string SQL = "";
            long.TryParse(DocID, out ID);
            try
            {

                if (ID != 0)
                {
                    SQL = @"update Master_Document set isdeleted=1 , deletedat=getdate(),deletedby=" + LoginID + " where DocID=" + DocID;
                    clsDataBaseHelper.ExecuteNonQuery(SQL);
                    //if (System.IO.Directory.Exists(WebsitePath + "\\" + DocTypeModal.DocTypeName))
                    //{
                    //    Request.Files[0].SaveAs(WebsitePath + "\\" + FilePath);
                    //}
                    Isreul = true;
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during DeleteDocumentJSON. The query was executed :", ex.ToString(), "", "CommonAjax", "CommonAjax", LoginID, IPAddress);
            }
            return Json(Isreul, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GenerateURLJSon(GetURLExitsResponse Modal)
        {
            PostResponse PostResult = new PostResponse();
            PostResult.SuccessMessage = "Action not saved";
            Modal.LoginID = LoginID;
            Modal.IPAddress = IPAddress;
            Modal.Doctype = clsCommon.Decrypt(Modal.Doctype);
            Modal.URL = clsCommon.RemoveSpecialCharters(Modal.URL);
            PostResult = Common_SPU.fnGetCheckURLExist(Modal);
            PostResult.AdditionalMessage = Modal.URL;
            return Json(PostResult, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UpdateColumn_CommonJson(GetUpdateColumnResponse Modal)
        {
            PostResponse PostResult = new PostResponse();
            PostResult.SuccessMessage = "Action not saved";
            Modal.LoginID = LoginID;
            Modal.IPAddress = IPAddress;
            PostResult = Common_SPU.fnGetUpdateColumnResponse(Modal);
            return Json(PostResult, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UpdateWebJson()
        {
            PostResponse PostResult = new PostResponse();
            PostResult.Status = clsApplicationSetting.CreateWeb_MenuJSon();
            PostResult.SuccessMessage = "Menu JSON Updated Successfully";
            return Json(PostResult, JsonRequestBehavior.AllowGet);
        }

    }
}