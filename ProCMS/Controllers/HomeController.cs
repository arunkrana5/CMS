using DataAccess.Models;
using DataAccess.ModelsMaster;
using DataAccess.ModelsMasterHelper;
using DataAccess.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProCMS.Controllers
{
    public class HomeController : Controller
    {
        GetWebResponse getWebResponse;
        IHomeHelper Home;
        IBlogHelper Blog;
        string DesignKey = clsApplicationSetting.GetWebConfigValue("DesignKey");
        long LoginID = 0;
        string IPAddress = "";
        public HomeController()
        {
            getWebResponse = new GetWebResponse();
            Home = new HomeModal();
            Blog = new BlogModal();
            IPAddress = clsApplicationSetting.GetIPAddress();
            getWebResponse.IPAddress = IPAddress;
            getWebResponse.LoginID = LoginID;
        }
        [HttpGet]
        public ActionResult MyAction(string Parameter1, string Parameter2)
        {
            string Temp1 = "", Temp2 = "";
            string MenuURLType = "";
            ViewBag.Parameter1 = Parameter1;
            ViewBag.Parameter2 = Parameter2;

            if (!string.IsNullOrEmpty(Parameter2))
            {
                Temp1 = Parameter1;
                if (Parameter1.Contains('-'))
                {
                    Temp1 = Parameter1.Split('-')[0];
                    Temp2 = Parameter1.Split('-')[1];
                    MenuURLType = Home.GetMenuURLType(Temp1);
                }
                else
                {
                    MenuURLType = Home.GetMenuURLType(Parameter1);
                }
            }
            else
            {

                MenuURLType = Home.GetMenuURLType(Parameter1);
            }

            switch (MenuURLType)
            {
                case "CMS":
                    List<CMSContent> Modal = new List<CMSContent>();
                    getWebResponse.Doctype = Parameter1;
                    Modal = Home.GetCMSContent(getWebResponse);
                    return View("CMSContent", Modal);

                case "GALLERY":
                    List<Gallery.Web> GalleryModal = new List<Gallery.Web>();
                    GalleryModal = Home.GetGalleryList_Web(0);
                    return View("GalleryContent", GalleryModal);

                case "BLOG":
                    if (string.IsNullOrEmpty(Parameter2))
                    {
                        List<Blog.Web> BlogModal = new List<Blog.Web>();
                        getWebResponse.Parameter2 = Parameter1;
                        BlogModal = Home.GetBlog_WebList(getWebResponse);
                        return View("~/Views/Home/BlogList.cshtml", BlogModal);
                    }
                    else
                    {
                        ViewBag.Temp1 = Temp1;
                        ViewBag.Temp2 = Temp2;
                        switch (Temp2)
                        {
                            case "Tags":
                                List<Blog.Web> BlogTags = new List<Blog.Web>();
                                getWebResponse.Doctype = "Tags";
                                getWebResponse.URL = Parameter2;
                                BlogTags = Home.GetBlogOthers_WebList(getWebResponse);
                                return View("~/Views/Home/BlogTags.cshtml", BlogTags);
                            case "Category":
                                List<Blog.Web> BlogCategory = new List<Blog.Web>();
                                getWebResponse.Doctype = "Category";
                                getWebResponse.URL = Parameter2;
                                BlogCategory = Home.GetBlogOthers_WebList(getWebResponse);

                                return View("~/Views/Home/BlogCategory.cshtml", BlogCategory);
                            case "Author":
                                List<Blog.Web> BlogAuthor = new List<Blog.Web>();
                                getWebResponse.Doctype = "Author";
                                getWebResponse.URL = Parameter2;
                                BlogAuthor = Home.GetBlogOthers_WebList(getWebResponse);

                                return View("~/Views/Home/BlogAuthor.cshtml", BlogAuthor);

                            case "Date":
                                List<Blog.Web> BlogDate = new List<Blog.Web>();
                                getWebResponse.Doctype = "Date";
                                getWebResponse.URL = Parameter2;
                                BlogDate = Home.GetBlogOthers_WebList(getWebResponse);
                                return View("~/Views/Home/BlogDate.cshtml", BlogDate);
                            default:
                                Blog.Details BlogDetails = new Blog.Details();
                                getWebResponse.Parameter1 = Parameter1;
                                BlogDetails = Home.GetBlogDetails_Web(getWebResponse);

                                ViewBag.twitterImage = clsApplicationSetting.GetBaseUrl() + BlogDetails.DetailImageURL;
                                ViewBag.twitterURL = clsApplicationSetting.GetBaseUrl() + BlogDetails.TypeURL + "/" + BlogDetails.BlogURL;
                                ViewBag.description = BlogDetails.ShortDesc;
                                ViewBag.twittertitle = BlogDetails.Heading;

                                //BlogDetails = Blog.GetBlogDetail(Parameter2);
                                //ViewBag.ExternalLinks = Home.GetExternalLinksList("N");
                                //ViewBag.BlogList = Blog.GetBlogList("", "Y");
                                //ViewBag.AllTags = Blog.GetAllTags();
                                return View("~/Views/Home/BlogDetails.cshtml", BlogDetails);
                        }
                    }

                default:
                    return RedirectToAction("Index");
            }
        }


        public ActionResult Index()
        {
            IndexModal Result = new IndexModal();
            Result = Home.GetIndexDetails();
            return View("Index" + DesignKey, Result);
        }

        public ActionResult ProductIndex(string Parameter1)
        {
            ViewBag.Parameter1 = Parameter1;
            GetURLExitsResponse_Web URLWebRes = new GetURLExitsResponse_Web();
            URLWebRes.Values = Parameter1;
            URLWebRes.Values = "CategoryURL";
            if (!string.IsNullOrEmpty(Parameter1) && Common_SPU.fnGetCheckURLExist_Web(URLWebRes).Status)
            {
                List<Product.Web> ProductwithCategory = new List<Product.Web>();
                ProductwithCategory = Home.GetProduct_WebList(Parameter1, "");
                return View("~/Views/Home/ProductList.cshtml", ProductwithCategory);
            }
            else
            {
                List<Category.Web> ProductwithCategory = new List<Category.Web>();
                ProductwithCategory = Home.GetCategory_WebList();
                return View("~/Views/Home/CategoryList.cshtml", ProductwithCategory);
            }

        }

        public ActionResult ProductDetail(string Parameter1)
        {
            Product.Web Modal = new Product.Web();
            Modal = Home.GetProduct_WebList("", Parameter1).FirstOrDefault();
            return View("~/Views/Home/ProductDetail.cshtml", Modal);
        }

        public ActionResult _ProductDemo(string PID)
        {
            long ID = 0;
            long.TryParse(PID, out ID);
            ViewBag.PID = PID;
            ProductDemo Modal = new ProductDemo();
            Modal.ProductID = ID;
            return PartialView(Modal);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult _ProductDemo(ProductDemo Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            Result.SuccessMessage = "Action not saved";


            if (ModelState.IsValid)
            {
                if (Command == "Add")
                {
                    Modal.LoginID = LoginID;
                    Modal.IPAddress = IPAddress;
                    Result = Common_SPU.fnSeQuery_ProductDemo(Modal);
                }
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BlogList()
        {
            List<Blog.Web> BlogModal = new List<Blog.Web>();

            GetWebResponse getModal = new GetWebResponse();
            getModal.ShowAll = "Y";
            BlogModal = Home.GetBlog_WebList(getModal);
            return View(BlogModal);
        }


        public ActionResult ContactUs(string Type)
        {
            Query.Add MyQuery = new Query.Add();
            MyQuery.Type = Type;
            ViewBag.Type = Type;
            if (string.IsNullOrEmpty(DesignKey))
            {
                return PartialView("ContactUs", MyQuery);
            }
            else
            {
                return PartialView("ContactUs_" + DesignKey, MyQuery);
            }
        }

        [HttpPost]


        public ActionResult ContactUs(Query.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            Result.SuccessMessage = "Query is not Submmited";

            if (ModelState.IsValid)
            {

                if (Command == "Add")
                {
                    Modal.LoginID = LoginID;
                    Modal.IPAddress = IPAddress;
                    Result = Common_SPU.fnSetQuery(Modal);
                }

            }


            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GalleryDetails(string GalleryGroupID)
        {
            long GroupID = 0;
            long.TryParse(clsCommon.Decrypt(GalleryGroupID), out GroupID);
            ViewBag.GroupID = GroupID;
            List<Gallery.Web> GalleryModal = new List<Gallery.Web>();
            GalleryModal = Home.GetGalleryList_Web(0);
            return View(GalleryModal);
        }

    }
}