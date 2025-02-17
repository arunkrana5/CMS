using DataAccess.Models;
using DataAccess.ModelsMaster;
using DataAccess.ModelsMasterHelper;
using ProCMS.Areas.Admin.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProCMS.Areas.Admin.Controllers
{
    [AuthorizeFilter]
    public class MastersController : Controller
    {
        IMasterHelper Master;
        long LoginID = 0;
        string IPAddress = "";
        GetResponse getResponse;
        public MastersController()
        {
            getResponse = new GetResponse();
            Master = new MasterModal();
            long.TryParse(clsApplicationSetting.GetSessionValue("LoginID"), out LoginID);
            IPAddress = clsApplicationSetting.GetIPAddress();
            getResponse.IPAddress = IPAddress;
            getResponse.LoginID = LoginID;
        }
        [HttpPost]
        public ActionResult _MenuURLType(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.URLType = GetQueryString[2];
            ViewBag.MenuURL = GetQueryString[3];
            getResponse.Doctype = ViewBag.URLType;
            List<WebMenu.Admin.MenuURLType> result = new List<WebMenu.Admin.MenuURLType>();
            result = Master.GetMenuURLTypeList(getResponse);
            return PartialView(result);

        }

        [HttpPost]
        public ActionResult _ChildMenuList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.WebMenuID = GetQueryString[2];
            long ID = 0;
            long.TryParse(GetQueryString[2], out ID);
            getResponse.ID = ID;
            List<WebMenu.Admin.ParentChildCollection> result = new List<WebMenu.Admin.ParentChildCollection>();
            result = Master.GetMenu_ParentChildCollectionList(getResponse);
            return PartialView(result);

        }

        public ActionResult WebsiteMenuList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<WebMenu.Admin.List> Modal = new List<WebMenu.Admin.List>();
            GetMenuResponse NewModal = new GetMenuResponse();
            NewModal.LoginID = LoginID;
            NewModal.IPAddress = IPAddress;
            NewModal.Doctype = "admin";
            Modal = Master.GetMaster_WebsiteMenuList(NewModal);
            return View(Modal);

        }

        public ActionResult AddWebsiteMenu(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.WebMenuID = GetQueryString[2];
            WebMenu.Admin.Add Modal = new WebMenu.Admin.Add();
            long ID = 0;
            long.TryParse(GetQueryString[2], out ID);
            if (ID > 0)
            {
                GetMenuResponse NewModal = new GetMenuResponse();
                NewModal.LoginID = LoginID;
                NewModal.IPAddress = IPAddress;
                NewModal.Doctype = "admin";
                NewModal.WebMenuID = ID;
                Modal = Master.GetMaster_WebsiteMenu(NewModal);
            }

            GetResponse GetRespo = new GetResponse();
            GetRespo.LoginID = LoginID;
            GetRespo.IPAddress = IPAddress;
            GetRespo.Doctype = "Parent";
            GetRespo.ID = ID;
            ViewBag.ParentMenuList = Master.GetMenu_ParentChildCollectionList(GetRespo);

            return View(Modal);

        }

        [HttpPost]
        public ActionResult AddWebsiteMenu(string src, WebMenu.Admin.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.WebMenuID = GetQueryString[2];
            long WebMenuID = 0;
            long.TryParse(ViewBag.WebMenuID, out WebMenuID);
            Result.SuccessMessage = "Menu Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Result = Master.fnSetWebiteMenu(Modal);

            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Masters/WebsiteMenuList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Masters/WebsiteMenuList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CMSList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<CMS.List> result = new List<CMS.List>();
            result = Master.GetCMSList(getResponse);
            return View(result);
        }
        public ActionResult _AddCMS(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.CMSID = GetQueryString[2];
            CMS.Add Modal = new CMS.Add();
            long ID = 0;
            long.TryParse(GetQueryString[2], out ID);
            if (ID > 0)
            {
                getResponse.ID = ID;
                Modal = Master.GetCMS(getResponse);
            }
            return PartialView(Modal);

        }

        [HttpPost]
        public ActionResult _AddCMS(string src, CMS.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.CMSID = GetQueryString[2];
            long CMSID = 0;
            long.TryParse(ViewBag.CMSID, out CMSID);
            Result.SuccessMessage = "CMS Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Result = Master.fnSetCMS(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Masters/CMSList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Masters/CMSList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult CMSSectionList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.CMSID = GetQueryString[2];

            long CMSID = 0;
            long.TryParse(ViewBag.CMSID, out CMSID);

            List<CMSSection.List> result = new List<CMSSection.List>();
            getResponse.AdditionalID = CMSID;
            result = Master.GetCMSSectionList(getResponse);
            return View(result);
        }

        public ActionResult _AddCMSSection(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.CMSSectionID = GetQueryString[2];
            ViewBag.CMSID = GetQueryString[3];
            long CMSID = 0, CMSSectionID = 0;
            long.TryParse(ViewBag.CMSID, out CMSID);
            long.TryParse(ViewBag.CMSSectionID, out CMSSectionID);

            CMSSection.Add Modal = new CMSSection.Add();
            if (CMSSectionID > 0)
            {
                getResponse.ID = CMSSectionID;
                getResponse.AdditionalID = CMSID;
                Modal = Master.GetCMSSection(getResponse);
            }
            return PartialView(Modal);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult _AddCMSSection(string src, CMSSection.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.CMSSectionID = GetQueryString[2];
            ViewBag.CMSID = GetQueryString[3];
            long CMSID = 0, CMSSectionID = 0;
            long.TryParse(ViewBag.CMSID, out CMSID);
            long.TryParse(ViewBag.CMSSectionID, out CMSSectionID);
            Result.SuccessMessage = "CMS Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.CMSID = CMSID;
                Result = Master.fnSetCMSSection(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Masters/CMSSectionList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Masters/CMSSectionList*" + CMSID);
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DocumentTypeList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<DocumentType.List> result = new List<DocumentType.List>();
            result = Master.GetDocumentTypeList(getResponse);
            return View(result);
        }

        public ActionResult _AddDocumentType(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.DocTypeID = GetQueryString[2];
            DocumentType.Add Modal = new DocumentType.Add();
            long DocTypeID = 0;
            long.TryParse(GetQueryString[2], out DocTypeID);
            if (DocTypeID > 0)
            {
                getResponse.ID = DocTypeID;
                Modal = Master.GetDocumentType(getResponse);
            }
            return PartialView(Modal);

        }

        [HttpPost]
        public ActionResult _AddDocumentType(string src, DocumentType.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.DocTypeID = GetQueryString[2];
            long DocTypeID = 0;
            long.TryParse(ViewBag.DocTypeID, out DocTypeID);
            Result.SuccessMessage = "Document Type Can't Update";
            string Path = clsApplicationSetting.GetPhysicalPath("") + "\\" + Modal.DocTypeName;
            if (Modal.DocTypeID > 0 && System.IO.Directory.Exists(Path))
            {
                Result.SuccessMessage = "Directory Already Exits !";
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;

                Result = Master.fnSetDocumentType(Modal);
            }
            if (Result.Status)
            {

                if (!System.IO.Directory.Exists(Path))
                {
                    System.IO.Directory.CreateDirectory(Path);
                }

                Result.RedirectURL = "/Admin/Masters/DocumentTypeList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Masters/DocumentTypeList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }



        public ActionResult BannerList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<Banner.Admin.List> result = new List<Banner.Admin.List>();
            result = Master.GetBannerList(getResponse);
            return View(result);
        }
        public ActionResult BannerAdd(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.BannerID = GetQueryString[2];
            Banner.Admin.Add Modal = new Banner.Admin.Add();
            long ID = 0;
            long.TryParse(GetQueryString[2], out ID);
            if (ID > 0)
            {
                getResponse.LoginID = LoginID;
                getResponse.IPAddress = IPAddress;
                getResponse.ID = ID;
                Modal = Master.GetBanner(getResponse);
            }
            return View(Modal);

        }
        [HttpPost]
        public ActionResult BannerAdd(string src, Banner.Admin.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.BannerID = GetQueryString[2];
            long BannerID = 0;
            long.TryParse(ViewBag.BannerID, out BannerID);
            Result.SuccessMessage = "Banner Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.BannerID = BannerID;
                Result = Master.fnSetBanner(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Masters/BannerList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Masters/BannerList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult TestimonialsList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<Testimonials.Admin.List> result = new List<Testimonials.Admin.List>();
            result = Master.GetTestimonialsList(getResponse);
            return View(result);
        }
        public ActionResult TestimonialsAdd(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.TestimonialID = GetQueryString[2];
            Testimonials.Admin.Add Modal = new Testimonials.Admin.Add();
            long ID = 0;
            long.TryParse(GetQueryString[2], out ID);
            if (ID > 0)
            {
                getResponse.LoginID = LoginID;
                getResponse.IPAddress = IPAddress;
                getResponse.ID = ID;
                Modal = Master.GetTestimonials(getResponse);
            }
            return View(Modal);

        }
        [HttpPost]
        public ActionResult TestimonialsAdd(string src, Testimonials.Admin.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.TestimonialID = GetQueryString[2];
            long TestimonialID = 0;
            long.TryParse(ViewBag.TestimonialID, out TestimonialID);
            Result.SuccessMessage = "Banner Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.TestimonialID = TestimonialID;
                Result = Master.fnSetTestimonials(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Masters/TestimonialsList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Masters/TestimonialsList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }



        public ActionResult ExternalLinksList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<ExternalLinks.Admin.List> result = new List<ExternalLinks.Admin.List>();
            result = Master.GetExternalLinksList(getResponse);
            return View(result);
        }
        public ActionResult AddExternalLink(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.LinkID = GetQueryString[2];
            ExternalLinks.Admin.Add Modal = new ExternalLinks.Admin.Add();
            long ID = 0;
            long.TryParse(GetQueryString[2], out ID);
            if (ID > 0)
            {
                getResponse.LoginID = LoginID;
                getResponse.IPAddress = IPAddress;
                getResponse.ID = ID;
                Modal = Master.GetExternalLinks(getResponse);
            }
            return View(Modal);

        }
        [HttpPost]
        public ActionResult AddExternalLink(string src, ExternalLinks.Admin.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.LinkID = GetQueryString[2];
            long LinkID = 0;
            long.TryParse(ViewBag.LinkID, out LinkID);
            Result.SuccessMessage = "External Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.LinkID = LinkID;
                Result = Master.fnSetExternalLinks(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Masters/ExternalLinksList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Masters/ExternalLinksList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }



        public ActionResult FAQList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<FAQ.Admin.List> result = new List<FAQ.Admin.List>();
            result = Master.GetFAQList(getResponse);
            return View(result);
        }
        public ActionResult FAQAdd(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.LinkID = GetQueryString[2];
            FAQ.Admin.Add Modal = new FAQ.Admin.Add();
            long ID = 0;
            long.TryParse(GetQueryString[2], out ID);
            if (ID > 0)
            {
                getResponse.LoginID = LoginID;
                getResponse.IPAddress = IPAddress;
                getResponse.ID = ID;
                Modal = Master.GetFAQ(getResponse);
            }
            return View(Modal);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FAQAdd(string src, FAQ.Admin.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.FAQID = GetQueryString[2];
            long FAQID = 0;
            long.TryParse(ViewBag.FAQID, out FAQID);
            Result.SuccessMessage = "FAQ Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.FAQID = FAQID;
                Result = Master.fnSetFAQ(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Masters/FAQList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Masters/FAQList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GalleryGroupList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<GalleryGroup.Admin.List> result = new List<GalleryGroup.Admin.List>();
            result = Master.GetGalleryGroupList(getResponse);
            return View(result);
        }
        public ActionResult GalleryGroupAdd(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.GalleryGroupID = GetQueryString[2];
            GalleryGroup.Admin.Add Modal = new GalleryGroup.Admin.Add();
            long GalleryGroupID = 0;
            long.TryParse(GetQueryString[2], out GalleryGroupID);
            if (GalleryGroupID > 0)
            {
                getResponse.LoginID = LoginID;
                getResponse.IPAddress = IPAddress;
                getResponse.ID = GalleryGroupID;
                Modal = Master.GetGalleryGroup(getResponse);
            }
            return View(Modal);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GalleryGroupAdd(string src, GalleryGroup.Admin.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.GalleryGroupID = GetQueryString[2];
            long GalleryGroupID = 0;
            long.TryParse(GetQueryString[2], out GalleryGroupID);
            Result.SuccessMessage = "Group Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.GalleryGroupID = GalleryGroupID;
                Result = Master.fnSetGalleryGroup(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Masters/GalleryGroupList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Masters/GalleryGroupList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }



        public ActionResult GalleryList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<Gallery.Admin.List> result = new List<Gallery.Admin.List>();
            result = Master.GetGalleryList(getResponse);
            return View(result);
        }
        public ActionResult GalleryAdd(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.GalleryID = GetQueryString[2];
            Gallery.Admin.Add Modal = new Gallery.Admin.Add();
            long GalleryID = 0;
            long.TryParse(GetQueryString[2], out GalleryID);
            if (GalleryID > 0)
            {
                getResponse.LoginID = LoginID;
                getResponse.IPAddress = IPAddress;
                getResponse.ID = GalleryID;
                Modal = Master.GetGallery(getResponse);
            }
            GetResponse get_R = new GetResponse();
            get_R.LoginID = LoginID;
            get_R.IPAddress = IPAddress;
            ViewBag.GalleryGroupList = Master.GetGalleryGroupList(get_R).Where(x => x.IsActive).ToList();
            return View(Modal);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GalleryAdd(string src, Gallery.Admin.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.GalleryID = GetQueryString[2];
            long GalleryID = 0;
            long.TryParse(GetQueryString[2], out GalleryID);
            Result.SuccessMessage = "Gallery Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.GalleryID = GalleryID;
                Result = Master.fnSetGallery(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Masters/GalleryList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Masters/GalleryList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult QueryList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<Query.List> Modal = new List<Query.List>();
            Modal = Master.GetQueryList(getResponse);
            return View(Modal);
        }

        public ActionResult MetaTagsList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<MetaTags.Admin.List> Modal = new List<MetaTags.Admin.List>();
            Modal = Master.GetMetaTagsList(getResponse);
            return View(Modal);
        }
        public ActionResult _MetaTagsAdd(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.CMSID = GetQueryString[2];
            MetaTags.Admin.Add Modal = new MetaTags.Admin.Add();
            long ID = 0;
            long.TryParse(GetQueryString[2], out ID);
            if (ID > 0)
            {
                getResponse.ID = ID;
                Modal = Master.GetMetaTags(getResponse);
            }
            return PartialView(Modal);

        }
        [HttpPost]
        public ActionResult _MetaTagsAdd(string src, MetaTags.Admin.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.GalleryID = GetQueryString[2];
            long ID = 0;
            long.TryParse(GetQueryString[2], out ID);
            Result.SuccessMessage = "Meta Tag Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.MetaTagsID = ID;
                Result = Master.fnSetMetaTags(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Masters/MetaTagsList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Masters/MetaTagsList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ClientList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<Client.Admin.List> Modal = new List<Client.Admin.List>();
            Modal = Master.GetClientList(getResponse);
            return View(Modal);

        }
        public ActionResult ClientAdd(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.ClientID = GetQueryString[2];
            long ID = 0;
            long.TryParse(ViewBag.ClientID, out ID);
            Client.Admin.Add Modal = new Client.Admin.Add();
            if (ID > 0)
            {
                getResponse.ID = ID;
                Modal = Master.GetClient(getResponse);
            }
            return View(Modal);

        }

        [HttpPost]
        public ActionResult ClientAdd(string src, Client.Admin.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.ClientID = GetQueryString[2];
            long ID = 0;
            long.TryParse(ViewBag.ClientID, out ID);
            Result.SuccessMessage = "Client Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.ClientID = ID;
                Result = Master.fnSetClient(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Masters/ClientList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Masters/ClientList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult CountryList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            GetMasterResponse getMasterResponse = new GetMasterResponse();
            getMasterResponse.LoginID = LoginID;
            getMasterResponse.IPAddress = IPAddress;
            getMasterResponse.TableName = "Country";
            getMasterResponse.IsActive = "0,1";
            List<MasterAll.Admin.List> Modal = new List<MasterAll.Admin.List>();
            Modal = Master.GetMasterAllList(getMasterResponse);
            return View(Modal);
        }

        public ActionResult _AddCountry(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.ID = GetQueryString[2];
            string TableName = "Country";
            long ID = 0;
            long.TryParse(ViewBag.ID, out ID);
            MasterAll.Admin.Add Modal = new MasterAll.Admin.Add();
            if (ID > 0)
            {
                GetMasterResponse getMasterResponse = new GetMasterResponse();
                getMasterResponse.LoginID = LoginID;
                getMasterResponse.IPAddress = IPAddress;
                getMasterResponse.TableName = TableName;
                getMasterResponse.ID = ID;
                Modal = Master.GetMasterAll(getMasterResponse);
            }
            Modal.table_name = TableName;
            return PartialView(Modal);
        }

        public ActionResult StateList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            GetMasterResponse getMasterResponse = new GetMasterResponse();
            getMasterResponse.LoginID = LoginID;
            getMasterResponse.IPAddress = IPAddress;
            getMasterResponse.TableName = "State";
            getMasterResponse.IsActive = "0,1";
            List<MasterAll.Admin.List> Modal = new List<MasterAll.Admin.List>();
            Modal = Master.GetMasterAllList(getMasterResponse);
            return View(Modal);
        }

        public ActionResult _AddState(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.ID = GetQueryString[2];
            string TableName = "State";
            long ID = 0;
            long.TryParse(ViewBag.ID, out ID);
            MasterAll.Admin.Add Modal = new MasterAll.Admin.Add();
            if (ID > 0)
            {
                GetMasterResponse getMasterResponse = new GetMasterResponse();
                getMasterResponse.LoginID = LoginID;
                getMasterResponse.IPAddress = IPAddress;
                getMasterResponse.TableName = TableName;
                getMasterResponse.ID = ID;
                Modal = Master.GetMasterAll(getMasterResponse);
            }
            Modal.table_name = TableName;

            GetMasterResponse getCountry = new GetMasterResponse();
            getCountry.LoginID = LoginID;
            getCountry.IPAddress = IPAddress;
            getCountry.TableName = "Country";
            getCountry.IsActive = "1";
            ViewBag.List = Master.GetMasterAllList(getCountry);

            return PartialView(Modal);
        }



        public ActionResult CityList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            GetMasterResponse getMasterResponse = new GetMasterResponse();
            getMasterResponse.LoginID = LoginID;
            getMasterResponse.IPAddress = IPAddress;
            getMasterResponse.TableName = "City";
            getMasterResponse.IsActive = "0,1";
            List<MasterAll.Admin.List> Modal = new List<MasterAll.Admin.List>();
            Modal = Master.GetMasterAllList(getMasterResponse);
            return View(Modal);
        }

        public ActionResult _AddCity(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.ID = GetQueryString[2];
            string TableName = "City";
            long ID = 0;
            long.TryParse(ViewBag.ID, out ID);
            MasterAll.Admin.Add Modal = new MasterAll.Admin.Add();
            if (ID > 0)
            {
                GetMasterResponse getMasterResponse = new GetMasterResponse();
                getMasterResponse.LoginID = LoginID;
                getMasterResponse.IPAddress = IPAddress;
                getMasterResponse.TableName = TableName;
                getMasterResponse.ID = ID;
                Modal = Master.GetMasterAll(getMasterResponse);
            }
            Modal.table_name = TableName;

            GetMasterResponse getCountry = new GetMasterResponse();
            getCountry.LoginID = LoginID;
            getCountry.IPAddress = IPAddress;
            getCountry.TableName = "State";
            getCountry.IsActive = "1";
            ViewBag.List = Master.GetMasterAllList(getCountry);

            return PartialView(Modal);
        }


        public ActionResult AreaList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            GetMasterResponse getMasterResponse = new GetMasterResponse();
            getMasterResponse.LoginID = LoginID;
            getMasterResponse.IPAddress = IPAddress;
            getMasterResponse.TableName = "Area";
            getMasterResponse.IsActive = "0,1";
            List<MasterAll.Admin.List> Modal = new List<MasterAll.Admin.List>();
            Modal = Master.GetMasterAllList(getMasterResponse);
            return View(Modal);
        }

        public ActionResult _AddArea(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.ID = GetQueryString[2];
            string TableName = "Area";
            long ID = 0;
            long.TryParse(ViewBag.ID, out ID);
            MasterAll.Admin.Add Modal = new MasterAll.Admin.Add();
            if (ID > 0)
            {
                GetMasterResponse getMasterResponse = new GetMasterResponse();
                getMasterResponse.LoginID = LoginID;
                getMasterResponse.IPAddress = IPAddress;
                getMasterResponse.TableName = TableName;
                getMasterResponse.ID = ID;
                Modal = Master.GetMasterAll(getMasterResponse);
            }
            Modal.table_name = TableName;

            GetMasterResponse getCountry = new GetMasterResponse();
            getCountry.LoginID = LoginID;
            getCountry.IPAddress = IPAddress;
            getCountry.TableName = "City";
            getCountry.IsActive = "1";
            ViewBag.List = Master.GetMasterAllList(getCountry);

            return PartialView(Modal);
        }



        [HttpPost]
        public ActionResult SaveMasterAll(string src, MasterAll.Admin.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.ID = GetQueryString[2];
            long ID = 0;
            long.TryParse(ViewBag.ID, out ID);
            Result.SuccessMessage = "Masters Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.ID = ID;
                Result = Master.fnSetMasterAll(Modal);
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }

    }
}