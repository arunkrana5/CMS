using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProCMS.Controllers
{
    public class AjaxController : Controller
    {
       
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
    }
}