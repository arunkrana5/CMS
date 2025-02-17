using DataAccess.Models;
using DataAccess.ModelsMaster;
using DataAccess.ModelsMasterHelper;
using DataAccess.Setting;
using ProCMS.Areas.Admin.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProCMS.Areas.Admin.Controllers
{
    [AuthorizeFilter]
    public class BlogController : Controller
    {
        IBlogHelper Blog;
        long LoginID = 0;
        string IPAddress = "";
        GetResponse getResponse;
        public BlogController()
        {
            getResponse = new GetResponse();
            Blog = new BlogModal();
            long.TryParse(clsApplicationSetting.GetSessionValue("LoginID"), out LoginID);
            IPAddress = clsApplicationSetting.GetIPAddress();
            getResponse.IPAddress = IPAddress;
            getResponse.LoginID = LoginID;
        }
        public ActionResult BlogTypeList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<BlogType.Admin.List> result = new List<BlogType.Admin.List>();
            result = Blog.GetBlogTypeList(getResponse);
            return View(result);
        }
        public ActionResult _AddBlogType(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.BlogTypeID = GetQueryString[2];
            BlogType.Admin.Add result = new BlogType.Admin.Add();
            long ID = 0;
            long.TryParse(ViewBag.BlogTypeID, out ID);
            if (ID > 0)
            {
                getResponse.ID = ID;
                result = Blog.GetBlogType(getResponse);
            }
            return PartialView(result);

        }
        [HttpPost]
        public ActionResult _AddBlogType(string src, BlogType.Admin.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.BlogTypeID = GetQueryString[2];
            long BlogTypeID = 0;
            long.TryParse(ViewBag.BlogTypeID, out BlogTypeID);
            Result.SuccessMessage = "Type Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Result = Blog.fnSetBlogType(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Blog/BlogTypeList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Blog/BlogTypeList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult BlogTagList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<BlogTags.Admin.List> result = new List<BlogTags.Admin.List>();
            result = Blog.GetBlogTagsList(getResponse);
            return View(result);
        }
        public ActionResult _AddBlogTag(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.BlogTagID = GetQueryString[2];
            BlogTags.Admin.Add result = new BlogTags.Admin.Add();
            long ID = 0;
            long.TryParse(ViewBag.BlogTagID, out ID);
            if (ID > 0)
            {
                getResponse.ID = ID;
                result = Blog.GetBlogTags(getResponse);
            }
            return PartialView(result);

        }
        [HttpPost]
        public ActionResult _AddBlogTag(string src, BlogTags.Admin.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.BlogTagID = GetQueryString[2];
            long BlogTagID = 0;
            long.TryParse(ViewBag.BlogTagID, out BlogTagID);
            Result.SuccessMessage = "Tags Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.BlogTagID = BlogTagID;
                Result = Blog.fnSetBlogTags(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Blog/BlogTagList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Blog/BlogTagList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }



        public ActionResult BlogCategoryList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<BlogCategory.Admin.List> result = new List<BlogCategory.Admin.List>();
            result = Blog.GetBlogCategoryList(getResponse);
            return View(result);
        }
        public ActionResult BlogCategoryAdd(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.BlogTagID = GetQueryString[2];
            BlogCategory.Admin.Add result = new BlogCategory.Admin.Add();
            long ID = 0;
            long.TryParse(ViewBag.BlogTagID, out ID);
            if (ID > 0)
            {
                getResponse.ID = ID;
                result = Blog.GetBlogCategory(getResponse);
            }
            return View(result);

        }
        [HttpPost]
        public ActionResult BlogCategoryAdd(string src, BlogCategory.Admin.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.blogCategoryID = GetQueryString[2];
            long blogCategoryID = 0;
            long.TryParse(ViewBag.blogCategoryID, out blogCategoryID);
            Result.SuccessMessage = "Blog Category Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.BlogCategoryID = blogCategoryID;
                Result = Blog.fnSetBlogCategory(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Blog/BlogCategoryList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Blog/BlogCategoryList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }



        public ActionResult BlogList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<Blog.Admin.List> result = new List<Blog.Admin.List>();
            result = Blog.GetBlogList(getResponse);
            return View(result);
        }
        public ActionResult BlogAdd(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.BlogID = GetQueryString[2];
            Blog.Admin.Add result = new Blog.Admin.Add();
            long ID = 0;
            long.TryParse(ViewBag.BlogID, out ID);
            if (ID > 0)
            {
                getResponse.ID = ID;
                result = Blog.GetBlog(getResponse);
            }

            GetResponse getResponse_Category = new GetResponse();
            getResponse_Category.IPAddress = IPAddress;
            getResponse_Category.LoginID = LoginID;

            GetResponse getResponse_Tag = new GetResponse();
            getResponse_Tag.IPAddress = IPAddress;
            getResponse_Tag.LoginID = LoginID;

            GetResponse getResponse_Type = new GetResponse();
            getResponse_Type.IPAddress = IPAddress;
            getResponse_Type.LoginID = LoginID;

            ViewBag.BlogCategoryList = Blog.GetBlogCategoryList(getResponse_Category).Where(x => x.IsActive).ToList();
            ViewBag.BlogTagList = Blog.GetBlogTagsList(getResponse_Tag).Where(x => x.IsActive).ToList();
            ViewBag.BlogTypeList = Blog.GetBlogTypeList(getResponse_Type).Where(x => x.IsActive).ToList();

            return View(result);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BlogAdd(string src, Blog.Admin.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.BlogID = GetQueryString[2];
            long BlogID = 0;
            long.TryParse(ViewBag.BlogID, out BlogID);
            Result.SuccessMessage = "Blog Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.BlogID = BlogID;
                Result = Blog.fnSetBlog(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Blog/BlogList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Blog/BlogList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }


    }
}