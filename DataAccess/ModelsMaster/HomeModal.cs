using Dapper;
using DataAccess.Models;
using DataAccess.ModelsMasterHelper;
using DataAccess.Setting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ModelsMaster
{
    public class HomeModal: IHomeHelper
    {
        string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();

        public List<WebMenu.List> GetMenu_Web(GetMenuResponse modal)
        {
            List<WebMenu.List> result = new List<WebMenu.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@WebMenuID", dbType: DbType.Int32, value: modal.WebMenuID, direction: ParameterDirection.Input);
                    param.Add("@Doctype", dbType: DbType.String, value: modal.Doctype??"", direction: ParameterDirection.Input);
                    param.Add("@LoginId", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                    param.Add("@MenuType", dbType: DbType.String, value: modal.MenuType??"", direction: ParameterDirection.Input);
                    param.Add("@ParentMenuID", dbType: DbType.Int32, value: modal.ParentMenuID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetMaster_WebsiteMenu", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<WebMenu.List>().ToList();
                    }
                    DBContext.Close();

                    if(result !=null)
                    {
                        foreach (var item in result.Where(x=>x.IsChild=="Y").ToList())
                        {
                                GetMenuResponse AgainRecursion = new GetMenuResponse();
                                AgainRecursion.LoginID = modal.LoginID;
                                AgainRecursion.IPAddress = modal.IPAddress;
                                AgainRecursion.ParentMenuID = item.WebMenuID;
                                item.ChildMenuList = GetMenu_Web(AgainRecursion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetWebsiteMenu. The query was executed :", ex.ToString(), "spu_GetMaster_WebsiteMenu()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public List<CMSContent> GetCMSContent(GetWebResponse modal)
        {
            List<CMSContent> result = new List<CMSContent>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@PageURL", dbType: DbType.String, value: modal.Doctype??"", direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetCMSContent", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<CMSContent>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetCMSContent. The query was executed :", ex.ToString(), "spu_GetCMSContent()", "MasterModal", "MasterModal", 0, "");

            }
            return result;
        }

        public List<Banner.Web> GetBanner_WebList(GetBannerResponse modal)
        {
            List<Banner.Web> result = new List<Banner.Web>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@BannerType", dbType: DbType.Int32, value: modal.BannerType, direction: ParameterDirection.Input);
                    param.Add("@InnerPageURL", dbType: DbType.Int32, value: modal.InnerPageURL, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetBanner_Web", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Banner.Web>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetBanner_WebList. The query was executed :", ex.ToString(), "spu_GetBanner_Web()", "HomeModal", "HomeModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public List<Testimonials.List> GetTestimonialsList(GetResponse modal)
        {
            List<Testimonials.List> result = new List<Testimonials.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                 
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetTestimonial_Web", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Testimonials.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetTestimonialsList. The query was executed :", ex.ToString(), "spu_GetTestimonial_Web()", "HomeModal", "HomeModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public List<ExternalLinks.List> GetExternalLinksList(GetResponse modal)
        {
            List<ExternalLinks.List> result = new List<ExternalLinks.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();

                    DBContext.Open();
                    param.Add("@LinkID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@AllLinks", dbType: DbType.Int32, value: modal.Doctype, direction: ParameterDirection.Input);
                    using (var reader = DBContext.QueryMultiple("spu_GetExternalLinks_Web", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<ExternalLinks.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetExternalLinksList. The query was executed :", ex.ToString(), "spu_GetExternalLinks_Web()", "HomeModal", "HomeModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public List<FAQ.List> GetFAQList(GetResponse modal)
        {
            List<FAQ.List> result = new List<FAQ.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();

                    DBContext.Open();
                    param.Add("@FAQURL", dbType: DbType.String, value: modal.Doctype, direction: ParameterDirection.Input);
                    using (var reader = DBContext.QueryMultiple("spu_GetFAQ_Web", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<FAQ.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetFAQList. The query was executed :", ex.ToString(), "spu_GetFAQ_Web()", "HomeModal", "HomeModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }


        public List<MetaTags.Web> GetMetaTags_WebList(GetResponse modal)
        {
            List<MetaTags.Web> result = new List<MetaTags.Web>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();

                    DBContext.Open();
                    param.Add("@PageURL", dbType: DbType.String, value: modal.Doctype, direction: ParameterDirection.Input);
                    using (var reader = DBContext.QueryMultiple("spu_GetMetaTags_Web", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<MetaTags.Web>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetMetaTags_WebList. The query was executed :", ex.ToString(), "spu_GetMetaTags_Web()", "HomeModal", "HomeModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public string GetMenuURLType(string MenuURL)
        {
            string SQL = "", MenuURLType = "";
            try
            {
                SQL = @"select URLType from Master_WebsiteMenu where MenuURL='" + MenuURL+ "' and WebMenuID!=0 and isdeleted=0";
                MenuURLType = clsDataBaseHelper.ExecuteSingleResult(SQL).ToUpper();
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetMenuURLType. The query was executed :", ex.ToString(), SQL, "HomeModal", "HomeModal", 0, "");
            }
            return MenuURLType;
        }


        public List<Blog.Web> GetBlog_WebList(GetWebResponse modal)
        {
            List<Blog.Web> result = new List<Blog.Web>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();

                    DBContext.Open();
                    param.Add("@BlogURL", dbType: DbType.String, value: modal.Parameter1??"", direction: ParameterDirection.Input);
                    param.Add("@TypeURL", dbType: DbType.String, value: modal.Parameter2??"", direction: ParameterDirection.Input);
                    param.Add("@AllBlogs", dbType: DbType.String, value: modal.ShowAll??"", direction: ParameterDirection.Input);
                    using (var reader = DBContext.QueryMultiple("spu_GetBlog_Web", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Blog.Web>().ToList();
                    }
                    DBContext.Close();

                    if(result!=null)
                    {
                        foreach (var item in result)
                        {
                            GetResponse getResponse = new GetResponse();
                            getResponse.Doctype = "Tag";
                            getResponse.ID = item.BlogID;
                            item.AllTags = BlogRelatedInformationList(getResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetBlog_WebList. The query was executed :", ex.ToString(), "spu_GetBlog_Web()", "HomeModal", "HomeModal", 0, "");

            }
            return result;
        }


        public Blog.Details GetBlogDetails_Web(GetWebResponse modal)
        {
            Blog.Details result = new Blog.Details();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();

                    DBContext.Open();
                    param.Add("@BlogURL", dbType: DbType.String, value: modal.Parameter1 ?? "", direction: ParameterDirection.Input);
                    using (var reader = DBContext.QueryMultiple("spu_GetBlogDetail_Web", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Blog.Details>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetBlogDetails_Web. The query was executed :", ex.ToString(), "spu_GetBlog_Web()", "HomeModal", "HomeModal", 0, "");

            }
            return result;
        }
        public List<BlogRelatedInformation> BlogRelatedInformationList(GetResponse modal)
        {
            List<BlogRelatedInformation> result = new List<BlogRelatedInformation>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();

                    DBContext.Open();
                    param.Add("@BlogID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@Doctype", dbType: DbType.String, value: modal.Doctype ?? "", direction: ParameterDirection.Input);
                    using (var reader = DBContext.QueryMultiple("spu_GetBlog_RelatedInformation_Web", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<BlogRelatedInformation>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during BlogRelatedInformationList. The query was executed :", ex.ToString(), "spu_BlogRelatedInformation_Web()", "HomeModal", "HomeModal", 0, "");

            }
            return result;
        }

        public IndexModal GetIndexDetails()
        {
            IndexModal result = new IndexModal();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();

                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetIndexPageItems", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result.BannerList = reader.Read<Banner.Web>().ToList();
                       
                        if (!reader.IsConsumed)
                        {
                            result.CMSContent = reader.Read<CMSContent>().ToList();
                        }
                        if (!reader.IsConsumed)
                        {
                            result.ClientsList = reader.Read<Client.Web>().ToList();
                        }
                        if (!reader.IsConsumed)
                        {
                            result.ProductList = reader.Read<Product.Web>().ToList();
                            if (result.ProductList != null)
                            {
                                GetResponse getImageRes = new GetResponse();
                                foreach (var item in result.ProductList)
                                {
                                    getImageRes.ID = item.ProductID;
                                    item.ProductImagesList = GetProductImageList(getImageRes);
                                }
                            }
                        }
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetIndexDetails. The query was executed :", ex.ToString(), "spu_GetIndexPageItems()", "HomeModal", "HomeModal", 0, "");

            }
            return result;
        }


        public List<Blog.Web> GetBlogOthers_WebList(GetWebResponse modal)
        {
            List<Blog.Web> result = new List<Blog.Web>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();

                    DBContext.Open();
                    param.Add("@Doctype", dbType: DbType.String, value: modal.Doctype ?? "", direction: ParameterDirection.Input);
                    param.Add("@URL", dbType: DbType.String, value: modal.URL ?? "", direction: ParameterDirection.Input);
                    using (var reader = DBContext.QueryMultiple("spu_GetBlogOthers_Web", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Blog.Web>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetBlogOthers_WebList. The query was executed :", ex.ToString(), "spu_GetBlogOthers_Web()", "HomeModal", "HomeModal", 0, "");

            }
            return result;
        }

        public List<Product.Web> GetProduct_WebList(string CategoryURL, string ProductURL)
        {

            List<Product.Web> result = new List<Product.Web>();
            try
            {
                string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@CategoryURL", dbType: DbType.String, value: clsCommon.EnsureString(CategoryURL));
                    param.Add("@ProductURL", dbType: DbType.String, value: clsCommon.EnsureString(ProductURL), direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetProduct_Web", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Product.Web>().ToList();
                    }
                    DBContext.Close();
                }
                if (result != null)
                {
                    GetResponse getImageRes = new GetResponse();
                    foreach (var item in result)
                    {
                        getImageRes.ID = item.ProductID;
                        item.ProductImagesList = GetProductImageList(getImageRes);
                    }
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetProduct_WebList. The query was executed :", ex.ToString(), "spu_GetProduct_Web()", "HomeModal", "HomeModal", 0, "");

            }
            return result;
        }
        private List<ProductImages.Web> GetProductImageList(GetResponse modal)
        {

            List<ProductImages.Web> result = new List<ProductImages.Web>();
            try
            {
                string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@ProductID", dbType: DbType.Int64, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@ImageType", dbType: DbType.String, value: clsCommon.EnsureString(modal.Doctype), direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetProductImage_List", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<ProductImages.Web>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetProductImageList. The query was executed :", ex.ToString(), "spu_GetProduct_Image()", "ProductModal", "ProductModal", modal.LoginID, modal.IPAddress);
            }
            return result;

        }

        public List<Category.Web> GetCategory_WebList()
        {

            List<Category.Web> result = new List<Category.Web>();
            try
            {
                string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetCategory_web", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Category.Web>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetCategory_WebList. The query was executed :", ex.ToString(), "spu_GetCategory_web()", "HomeModal", "HomeModal", 0, "");

            }
            return result;
        }


        public List<Gallery.Web> GetGalleryList_Web(int IsShowHome = 0)
        {
            List<Gallery.Web> result = new List<Gallery.Web>();
            try
            {
                string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@IsShowHome", dbType: DbType.Int32, value: IsShowHome, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetGallery_Web", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Gallery.Web>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetGalleryList_Web. The query was executed :", ex.ToString(), "spu_GetGallery_Web()", "HomeModal", "HomeModal", 0, "");

            }
            return result;
        }
    }
}
