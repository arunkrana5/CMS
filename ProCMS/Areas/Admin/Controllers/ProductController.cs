using DataAccess.Models;
using DataAccess.ModelsMaster;
using DataAccess.ModelsMasterHelper;
using DataAccess.Setting;
using ProCMS.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProCMS.Areas.Admin.Controllers
{
    [AuthorizeFilter]
    public class ProductController : Controller
    {
        IProductHelper Product;
        long LoginID = 0;
        string IPAddress = "";
        GetResponse getResponse;
        public ProductController()
        {
            Product = new ProductModal();
            getResponse = new GetResponse();
            long.TryParse(clsApplicationSetting.GetSessionValue("LoginID"), out LoginID);
            IPAddress = clsApplicationSetting.GetIPAddress();
            getResponse.IPAddress = IPAddress;
            getResponse.LoginID = LoginID;
        }
        public ActionResult CategoryList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<Category.Admin.List> result = new List<Category.Admin.List>();
            result = Product.GetCategoryList(getResponse);
            return View(result);
        }
        public ActionResult CategoryAdd(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.CategoryID = GetQueryString[2];
            Category.Admin.Add result = new Category.Admin.Add();
            long CategoryID = 0;
            long.TryParse(ViewBag.CategoryID, out CategoryID);
            if (CategoryID > 0)
            {
                getResponse.ID = CategoryID;
                result = Product.GetCategory(getResponse);
            }
            return View(result);

        }
        [HttpPost]
        public ActionResult CategoryAdd(string src, Category.Admin.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.CategoryID = GetQueryString[2];
            long CategoryID = 0;
            long.TryParse(ViewBag.CategoryID, out CategoryID);
            Result.SuccessMessage = "Category Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.CategoryID = CategoryID;
                Result = Product.fnSetCategory(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Product/CategoryList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Product/CategoryList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }



        public ActionResult ProductList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<Product.Admin.List> result = new List<Product.Admin.List>();
            result = Product.GetProductList(getResponse);
            return View(result);
        }
        public ActionResult ProductAdd(string src)
        {
            ViewBag.TabIndex = 1;
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.ProductID = GetQueryString[2];
            Product.Admin.Add result = new Product.Admin.Add();
            long ProductID = 0;
            long.TryParse(ViewBag.ProductID, out ProductID);
            if (ProductID > 0)
            {
                getResponse.ID = ProductID;
                result = Product.GetProduct(getResponse);
            }
            GetResponse getDropDown = new GetResponse();
            getDropDown.Doctype = "Category";
            result.CategoryList = Common_SPU.GetDropDownList(getDropDown);
            return View(result);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProductAdd(string src, Product.Admin.Add Modal, string Command)
        {
            ViewBag.TabIndex = 1;
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.ProductID = GetQueryString[2];
            long ProductID = 0;
            long.TryParse(ViewBag.ProductID, out ProductID);
            Result.SuccessMessage = "Product Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.ProductID = ProductID;
                Result = Product.fnSetProduct(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Product/ProductAdd?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Product/ProductAdd*"+ Result.ID);
            }
            TempData["Success"] = "Y";
            TempData["SuccessMsg"] = Result.SuccessMessage;
            return Json(Result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ProductImages(string src)
        {
            ViewBag.TabIndex = 2;
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.ProductID = GetQueryString[2];
            long ProductID = 0;
            long.TryParse(ViewBag.ProductID, out ProductID);
            List<ProductImages.Admin.List> result = new List<ProductImages.Admin.List>();
            if (ProductID > 0)
            {
                getResponse.AdditionalID = ProductID;
                result = Product.GetProductImageList(getResponse);
            }
            return View(result);
        }
        public ActionResult ProductImagesAdd(string src)
        {
            ViewBag.TabIndex = 2;
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.ProductID = GetQueryString[2];
            ViewBag.ProductImageID = GetQueryString[3];
            long ProductID = 0, ProductImageID=0;
            long.TryParse(ViewBag.ProductID, out ProductID);
            long.TryParse(ViewBag.ProductImageID, out ProductImageID);
            ProductImages.Admin.Add result = new ProductImages.Admin.Add();
            if (ProductImageID > 0)
            {
                getResponse.ID = ProductImageID;
                getResponse.AdditionalID = ProductID;
                result = Product.GetProductImage(getResponse);
            }
            return View(result);

        }
        [HttpPost]
        public ActionResult ProductImagesAdd(string src, ProductImages.Admin.Add Modal, string Command)
        {
            ViewBag.TabIndex = 2;
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.ProductID = GetQueryString[2];
            ViewBag.ProductImageID = GetQueryString[3];
            long ProductID = 0, ProductImageID = 0;
            long.TryParse(ViewBag.ProductID, out ProductID);
            long.TryParse(ViewBag.ProductImageID, out ProductImageID);
            Result.SuccessMessage = "Product Image Can't Update";
            if (Modal.ImageType=="Video" && string.IsNullOrEmpty(Modal.ImageURL))
            {
                Result.SuccessMessage = "Video URL can't be blank";
                ModelState.AddModelError("ImageURL", Result.SuccessMessage);
               
            }
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Modal.ProductID = ProductID;
                Modal.ProductImageID = ProductImageID;
                Result = Product.fnSetProductImage(Modal);
            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Product/ProductImages?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Product/ProductImages*" + ProductID+"*"+ ProductImageID);
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }
    }
}