using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;

namespace ProCMS.Areas.Admin.Data
{
    public class AuthorizeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            long LoginID = 0;
            long.TryParse(clsApplicationSetting.GetSessionValue("LoginID"), out LoginID);
            

            string[] Requestedsrc = null;
            string actionName = (string)filterContext.RouteData.Values["action"];
            string CurrentSession = "";
            if (clsApplicationSetting.GetWebConfigValue("AllowMultipleLogin") != "true")
            { CurrentSession = clsDataBaseHelper.fnGetOther_FieldName("users", "SessionID", "ID", LoginID.ToString(), ""); }

            if (HttpContext.Current.Request.HttpMethod == "GET")
            {
                Requestedsrc = clsApplicationSetting.DecryptQueryString(HttpContext.Current.Request.QueryString["src"]);
            }
            else
            {
                if (filterContext.Controller.ValueProvider.GetValue("src") != null)
                {
                    Requestedsrc = clsApplicationSetting.DecryptQueryString(filterContext.Controller.ValueProvider.GetValue("src").AttemptedValue);
                }
            }

            if (HttpContext.Current.Session["LoginID"] == null)
            {
                string ReturnURL = HttpContext.Current.Request.Url.AbsoluteUri;
                HttpContext.Current.Response.Redirect("~/Admin/Accounts/Login?ReturnURL=" + clsCommon.Encrypt(ReturnURL));
            }
         
            else if (actionName.ToLower() != "dashboard" && Requestedsrc == null)
            {
                HttpContext.Current.Response.Redirect("~/Admin/Accounts/PageNotFound");
            }
            else if (actionName.ToLower() != "dashboard" && HttpContext.Current.Request.Url.AbsolutePath.ToLower() != Requestedsrc[1].ToLower())
            {
                HttpContext.Current.Response.Redirect("~/Admin/Accounts/PageNotFound");
            }
            else if (Requestedsrc != null && !clsApplicationSetting.CheckPageViewPermission(Requestedsrc[0]).ReadFlag)
            {
                HttpContext.Current.Response.Redirect("~/Admin/Accounts/PageNotFound");
            }
            else if (!string.IsNullOrEmpty(CurrentSession) && CurrentSession != filterContext.HttpContext.Session.SessionID.ToString())
            {
                HttpContext.Current.Response.Redirect("~/Admin/Accounts/Logout");
            }

        }
    }
}