using DataAccess.Models;
using DataAccess.Setting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProCMS.Areas.Admin.Controllers
{
    public class AccountsController : Controller
    {

        long LoginID = 0;
        string IPAddress = "";
        GetResponse getResponse;
        public AccountsController()
        {
            getResponse = new GetResponse();
            long.TryParse(clsApplicationSetting.GetSessionValue("LoginID"), out LoginID);
            IPAddress = clsApplicationSetting.GetIPAddress();
            getResponse.IPAddress = IPAddress;
            getResponse.LoginID = LoginID;
        }

        string DesignKey = clsApplicationSetting.GetWebConfigValue("DesignKey");
        public ActionResult Login(string ReturnURL)
        {
            ViewBag.ReturnURL = ReturnURL;
            AdminUser.Login Modal = new AdminUser.Login();
            return View("Login" + DesignKey, Modal);
        }

        [HttpPost]

        public ActionResult Login(AdminUser.Login Modal, string ReturnURL, string Command)
        {
            ViewBag.ReturnURL = ReturnURL;
            DataSet UserDataSet = default(DataSet);
            bool IsValidUser = false;
            if (ModelState.IsValid)
            {
                if (Command == "Submit")
                {
                    UserDataSet = Common_SPU.fnGetLogin(Modal.UserName, clsCommon.Encrypt(Modal.Password), HttpContext.Session.SessionID);
                    bool.TryParse(UserDataSet.Tables[0].Rows[0]["status"].ToString(), out IsValidUser);
                    if (IsValidUser)
                    {
                        clsApplicationSetting.SetSessionValue("LoginID", UserDataSet.Tables[0].Rows[0]["LoginID"].ToString());
                        clsApplicationSetting.SetSessionValue("UserID", UserDataSet.Tables[0].Rows[0]["USERID"].ToString());
                        clsApplicationSetting.SetSessionValue("UserName", UserDataSet.Tables[0].Rows[0]["USERNAME"].ToString());
                        clsApplicationSetting.SetSessionValue("RoleID", UserDataSet.Tables[0].Rows[0]["role_id"].ToString());
                        clsApplicationSetting.SetSessionValue("RoleName", UserDataSet.Tables[0].Rows[0]["ROLE_NAME"].ToString());

                        if (!string.IsNullOrEmpty(ReturnURL))
                        {
                            return Redirect(clsCommon.Decrypt(ReturnURL));
                        }
                        else
                        {
                            return RedirectToAction("Dashboard", "Index");

                        }
                    }
                    else
                    {
                        TempData["SuccessMsg"] = "User ID does not exist or User ID De-activated. Please Contact your system administrator";
                    }
                }
            }
            return View("Login" + DesignKey, Modal);

        }


        public ActionResult Logout()
        {
            clsApplicationSetting.ClearSessionValues();
            return RedirectToAction("Login", "Accounts");
        }
        public ActionResult PageNotFound()
        {
            return View();
        }

        public string Encryption(string Token, string Value)
        {
            string Output;
            string ActualToken = clsApplicationSetting.GetConfigValue("Token");

            if (Token == ActualToken)
            {
                Value = Value.Replace("-SLASH-", "/");
                Value = Value.Replace("-STAR-", "*");
                Output = clsCommon.Encrypt(Value);
            }
            else
            {
                Output = "Invalid Token";
            }
            return Output;
        }
        public string Decryption(string Token, string Value)
        {
            string Output;
            string ActualToken = clsApplicationSetting.GetConfigValue("Token");
            if (Token == ActualToken)
            {
                Output = clsCommon.Decrypt(Value);
            }
            else
            {
                Output = "Invalid Token";
            }
            return Output;
        }


    }
}