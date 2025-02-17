using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProCMS.Areas.Admin.Data
{
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            long LoginID = 0;
            long.TryParse(clsApplicationSetting.GetSessionValue("LoginID"), out LoginID);
          
            string CurrentSession = "";
            if (clsApplicationSetting.GetWebConfigValue("AllowMultipleLogin") != "true")
            { CurrentSession = clsDataBaseHelper.fnGetOther_FieldName("users", "SessionID", "ID", LoginID.ToString(), ""); }

            if (HttpContext.Current.Session["LoginID"] == null)
            {
                string ReturnURL = HttpContext.Current.Request.Url.AbsoluteUri;
                HttpContext.Current.Response.Redirect("~/Admin/Accounts/Login?ReturnURL=" + clsCommon.Encrypt(ReturnURL));
            }
            
            else if (!string.IsNullOrEmpty(CurrentSession) && CurrentSession != filterContext.HttpContext.Session.SessionID.ToString())
            {
                HttpContext.Current.Response.Redirect("~/Admin/Accounts/Logout");
            }

        }
    }
}