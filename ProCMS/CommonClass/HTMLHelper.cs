using DataAccess.Models;
using DataAccess.ModelsMaster;
using DataAccess.ModelsMasterHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProCMS.CommonClass
{
    public static class HTMLHelper
    {
        public static string DesignKey = clsApplicationSetting.GetWebConfigValue("DesignKey");
        public static string Version = clsApplicationSetting.GetWebConfigValue("Version");
        //string controllerName =(string)htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
        public static MvcHtmlString IncludeVersionedJs(this HtmlHelper helper, string filename)
        {
            return MvcHtmlString.Create("<script type='text/javascript' src='" + filename + "?v=" + Version + "'></script>");
        }
        public static MvcHtmlString IncludeVersionedCss(this HtmlHelper helper, string filename)
        {
            return MvcHtmlString.Create("<link href='" +  filename + "?v=" + Version + "' rel='stylesheet' />");

        }
        public static MvcHtmlString CreateImage(this HtmlHelper helper, string Link, string alt = "", string ClassName = "")
        {
          
            ClassName = (string.IsNullOrEmpty(ClassName) ? "" : "class=\"" + ClassName + "\"");
            alt = (string.IsNullOrEmpty(alt) ? "" : "alt=\"" + alt + "\"");
            

            try
            {
                if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(Link)))
                {
                    Link = "/assets/design"+ clsApplicationSetting.GetWebConfigValue("DesignKey") + "/images/NoImage.png";
                }
            }
            catch (Exception ex)
            {
                Link = "/assets/design" + clsApplicationSetting.GetWebConfigValue("DesignKey") + "/images/NoImage.png";
            }

            return MvcHtmlString.Create("<img src='" + Link + "?v=" + Version + "' " + ClassName + " " + alt + " />");
        }


        public static MvcHtmlString CreateNavigation(this HtmlHelper helper, string Type, string For, string ID, string Text, string ClassName, string Tooltip, string FAIcons, string Target)
        {
            string Link = "";
            Tooltip = (string.IsNullOrEmpty(Tooltip) ? "" : "data-toggle='tooltip' data-original-title=\"" + Tooltip + "\"");
            if (For.ToLower() == "back")
            {
                Text = (string.IsNullOrEmpty(FAIcons) ? Text : "<i class=\"" + FAIcons + "\" aria-hidden='true'></i> " + Text);
            }
            else
            {
                Text = (string.IsNullOrEmpty(FAIcons) ? Text : Text + "<i class=\"" + FAIcons + "\" aria-hidden='true'></i> ");
            }
            long IDD = 0, CurrentRow = 0;
            long.TryParse(ID, out IDD);
            IHomeHelper Home = new HomeModal();
            switch (Type.ToLower())
            {
                //case "faq":

                   
                //    List<FAQ> List = new List<FAQ>();
                //    List = Home.GetFAQList(0, "");
                //    CurrentRow = List.Where(x => x.FAQID == IDD).Select(x => x.RowNum).FirstOrDefault();
                //    if (For.ToLower() == "back")
                //    {
                //        CurrentRow = CurrentRow - 1;
                //        if (CurrentRow < 1 || CurrentRow > List.Count())
                //        {
                //            Link = "javascripts:;";
                //        }
                //        else
                //        {
                //            Link = "/FAQDetail/" + List.Where(x => x.RowNum == CurrentRow).Select(x => x.FAQURL).FirstOrDefault();
                //        }

                //    }
                //    else
                //    {
                //        CurrentRow = CurrentRow + 1;
                //        if (CurrentRow < 1 || CurrentRow > List.Count())
                //        {
                //            Link = "javascripts:;";
                //        }
                //        else
                //        {
                //            Link = "/FAQDetail/" + List.Where(x => x.RowNum == CurrentRow).Select(x => x.FAQURL).FirstOrDefault();
                //        }
                //    }
                //    break;
                case "blog":

                    GetWebResponse getWebResponse = new GetWebResponse();   
                    List<Blog.Web> BlogModal = new List<Blog.Web>();
                    BlogModal = Home.GetBlog_WebList(getWebResponse);
                    CurrentRow = BlogModal.Where(x => x.BlogID == IDD).Select(x => x.RowNum).FirstOrDefault();
                    if (For.ToLower() == "back")
                    {
                        CurrentRow = CurrentRow - 1;
                        if (CurrentRow < 1 || CurrentRow > BlogModal.Count())
                        {
                            Link = "javascripts:;";
                        }
                        else
                        {
                            string TypeURL = BlogModal.Where(x => x.RowNum == CurrentRow).Select(x => x.TypeURL).FirstOrDefault();
                            string BlogURL = BlogModal.Where(x => x.RowNum == CurrentRow).Select(x => x.BlogURL).FirstOrDefault();
                            Link = "/" + TypeURL + "/" + BlogURL;
                        }

                    }
                    else
                    {
                        CurrentRow = CurrentRow + 1;
                        if (CurrentRow < 1 || CurrentRow > BlogModal.Count())
                        {
                            Link = "javascripts:;";
                        }
                        else
                        {
                            string TypeURL = BlogModal.Where(x => x.RowNum == CurrentRow).Select(x => x.TypeURL).FirstOrDefault();
                            string BlogURL = BlogModal.Where(x => x.RowNum == CurrentRow).Select(x => x.BlogURL).FirstOrDefault();
                            Link = "/" + TypeURL + "/" + BlogURL;
                        }
                    }
                    break;
                default:
                    break;
            }

            return MvcHtmlString.Create("<a " + Tooltip + " target=\"" + Target + "\" class=\"" + ClassName + "\" href='" + Link + "'>" + Text + "</a>");
        }

    }
}