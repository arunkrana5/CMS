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
   public class MasterModal: IMasterHelper
    {
        string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();


        public List<WebMenu.Admin.ParentChildCollection> GetMenu_ParentChildCollectionList(GetResponse modal)
        {
            List<WebMenu.Admin.ParentChildCollection> result = new List<WebMenu.Admin.ParentChildCollection>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();

                    param.Add("@WebMenuID", dbType: DbType.String, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@Doctype", dbType: DbType.String, value:clsCommon.EnsureString(modal.Doctype), direction: ParameterDirection.Input);

                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetMenu_ParentChildCollection", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<WebMenu.Admin.ParentChildCollection>().ToList();
                    }
                    DBContext.Close();
                }
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        if (item.ParentMenuID != 0)
                        {
                            item.ParentMenuMap = GetParentMenuMap(item.ParentMenuID) + "" + item.MenuName;
                        }
                        if(item.IsChild=="Y")
                        {
                            GetResponse AgainRecursion = new GetResponse();
                            AgainRecursion.LoginID = modal.LoginID;
                            AgainRecursion.IPAddress = modal.IPAddress;
                            AgainRecursion.ID = item.WebMenuID;
                            item.ChildList = GetMenu_ParentChildCollectionList(AgainRecursion);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetMenu_ParentChildCollectionList. The query was executed :", ex.ToString(), "spu_GetMenu_ParentChildCollection()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public List<WebMenu.Admin.MenuURLType> GetMenuURLTypeList(GetResponse modal)
        {
            List<WebMenu.Admin.MenuURLType> result = new List<WebMenu.Admin.MenuURLType>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                   
                    param.Add("@Doctype", dbType: DbType.String, value: modal.Doctype, direction: ParameterDirection.Input);
                    param.Add("@LoginId", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                  
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetMenuURLType", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<WebMenu.Admin.MenuURLType>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetMenuURLTypeList. The query was executed :", ex.ToString(), "spu_GetMenuURLType()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public PostResponse fnSetWebiteMenu(WebMenu.Admin.Add model)
        {
            PostResponse result = new PostResponse();
            string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetMaster_WebsiteMenu", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@WebMenuID", SqlDbType.Int).Value = model.WebMenuID;
                        command.Parameters.Add("@MenuType", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.MenuType);
                        command.Parameters.Add("@MenuName", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.MenuName);
                        command.Parameters.Add("@ParentMenuID", SqlDbType.Int).Value = model.ParentMenuID;
                        command.Parameters.Add("@DocID", SqlDbType.Int).Value = model.DocID;
                        command.Parameters.Add("@URLType", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.URLType);
                        command.Parameters.Add("@MenuURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.MenuURL);
                        command.Parameters.Add("@Target", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Target.ToString());
                        command.Parameters.Add("@Priority", SqlDbType.Int).Value = model.Priority ?? 0;
                        command.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = model.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = model.IPAddress;

                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.ID = Convert.ToInt64(reader["RET_ID"]);
                                result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (result.StatusCode > 0)
                                {
                                    result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }

        public List<WebMenu.Admin.List> GetMaster_WebsiteMenuList(GetMenuResponse modal)
        {
            List<WebMenu.Admin.List> result = new List<WebMenu.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@WebMenuID", dbType: DbType.Int32, value: modal.WebMenuID, direction: ParameterDirection.Input);
                    param.Add("@Doctype", dbType: DbType.String, value: modal.Doctype, direction: ParameterDirection.Input);
                    param.Add("@LoginId", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                    param.Add("@MenuType", dbType: DbType.String, value: modal.MenuType, direction: ParameterDirection.Input);
                    param.Add("@ParentMenuID", dbType: DbType.Int32, value: modal.ParentMenuID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetMaster_WebsiteMenu", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<WebMenu.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        if (item.ParentMenuID != 0)
                        {
                            item.ParentMenuMap = GetParentMenuMap(item.ParentMenuID) + "" + item.MenuName;
                        }
                    }
                   
                }
               
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetMaster_WebsiteMenuList. The query was executed :", ex.ToString(), "spu_GetMaster_WebsiteMenu()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public WebMenu.Admin.Add GetMaster_WebsiteMenu(GetMenuResponse modal)
        {
            WebMenu.Admin.Add result = new WebMenu.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@WebMenuID", dbType: DbType.Int32, value: modal.WebMenuID, direction: ParameterDirection.Input);
                    param.Add("@Doctype", dbType: DbType.String, value: modal.Doctype, direction: ParameterDirection.Input);
                    param.Add("@LoginId", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                    param.Add("@MenuType", dbType: DbType.String, value: modal.MenuType, direction: ParameterDirection.Input);
                    param.Add("@ParentMenuID", dbType: DbType.Int32, value: modal.ParentMenuID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetMaster_WebsiteMenu", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<WebMenu.Admin.Add>().FirstOrDefault();
                      
                    }
                    DBContext.Close();
                }
                if(result!=null)
                {
                    if (result.ParentMenuID != 0)
                    {
                        result.ParentMenuMap = GetParentMenuMap(result.ParentMenuID) + "" + result.MenuName;
                    }
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetMaster_WebsiteMenu. The query was executed :", ex.ToString(), "spu_GetMaster_WebsiteMenu()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        private string GetParentMenuMap(long ParentMenuID)
        {
            string Sql = "";
            DataSet TempProductCategory = new DataSet();
            Sql = "select ParentMenuID,MenuName,WebMenuID from Master_WebsiteMenu  where WebMenuID=" + ParentMenuID + " and isdeleted=0 and IsActive=1  Order By Priority,MenuName";

            TempProductCategory = clsDataBaseHelper.ExecuteDataSet(Sql);
            foreach (DataRow dr in TempProductCategory.Tables[0].Rows)
            {
                return GetParentMenuMap(Convert.ToInt64(dr["ParentMenuID"])) + "" + dr["MenuName"] + "-->";
            }
            return "";
        }

        public PostResponse fnSetCMS(CMS.Add model)
        {
            PostResponse result = new PostResponse();
            string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetCMS", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@CMSID", SqlDbType.Int).Value = model.CMSID;
                        command.Parameters.Add("@Heading", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Heading);
                        command.Parameters.Add("@PageURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.PageURL);
                        command.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = model.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = model.IPAddress;

                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.ID = Convert.ToInt64(reader["RET_ID"]);
                                result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (result.StatusCode > 0)
                                {
                                    result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }

        public PostResponse fnSetCMSSection(CMSSection.Add model)
        {
            PostResponse result = new PostResponse();
            string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetCMSSection", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@CMSSectionID", SqlDbType.Int).Value = model.CMSSectionID;
                        command.Parameters.Add("@CMSID", SqlDbType.Int).Value = model.CMSID;
                        command.Parameters.Add("@SectionName", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.SectionName);
                        command.Parameters.Add("@Content", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Content);
                        command.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = model.LoginID;
                        command.Parameters.Add("@Priority", SqlDbType.Int).Value = model.Priority??0;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = model.IPAddress;

                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.ID = Convert.ToInt64(reader["RET_ID"]);
                                result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (result.StatusCode > 0)
                                {
                                    result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }


        public List<CMS.List> GetCMSList(GetResponse modal)
        {
            List<CMS.List> result = new List<CMS.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@CMSID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@LoginId", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetCMS", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<CMS.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetCMSList. The query was executed :", ex.ToString(), "spu_GetCMS()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public CMS.Add GetCMS(GetResponse modal)
        {
            CMS.Add result = new CMS.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@CMSID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@LoginId", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetCMS", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<CMS.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetCMS. The query was executed :", ex.ToString(), "spu_GetCMS()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }
        public List<CMSSection.List> GetCMSSectionList(GetResponse modal)
        {
            List<CMSSection.List> result = new List<CMSSection.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@CMSSectionID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@CMSID", dbType: DbType.Int32, value: modal.AdditionalID, direction: ParameterDirection.Input);
                    param.Add("@LoginId", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetCMSSection", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<CMSSection.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetCMSList. The query was executed :", ex.ToString(), "spu_GetCMS()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

     

        public CMSSection.Add GetCMSSection(GetResponse modal)
        {
            CMSSection.Add result = new CMSSection.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@CMSSectionID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@CMSID", dbType: DbType.Int32, value: modal.AdditionalID, direction: ParameterDirection.Input);
                    param.Add("@LoginId", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetCMSSection", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<CMSSection.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetCMSSection. The query was executed :", ex.ToString(), "spu_GetCMS()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public List<DocumentType.List> GetDocumentTypeList(GetResponse modal)
        {
            List<DocumentType.List> result = new List<DocumentType.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@DocTypeID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@LoginId", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetDocumentType", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<DocumentType.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetDocumentTypeList. The query was executed :", ex.ToString(), "spu_GetDocumentType()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public DocumentType.Add GetDocumentType(GetResponse modal)
        {
            DocumentType.Add result = new DocumentType.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@DocTypeID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@LoginId", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetDocumentType", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<DocumentType.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetDocumentType. The query was executed :", ex.ToString(), "spu_GetDocumentType()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public PostResponse fnSetDocumentType(DocumentType.Add model)
        {
            PostResponse result = new PostResponse();
            string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetDocumentType", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@DocTypeID", SqlDbType.Int).Value = model.DocTypeID;
                        command.Parameters.Add("@DocTypeName", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.DocTypeName);
                        command.Parameters.Add("@DisplayName", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.DisplayName);
                        command.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@Priority", SqlDbType.Int).Value = model.Priority??0;
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = model.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = model.IPAddress;

                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.ID = Convert.ToInt64(reader["RET_ID"]);
                                result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (result.StatusCode > 0)
                                {
                                    result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }



        public List<Banner.Admin.List> GetBannerList(GetResponse modal)
        {
            List<Banner.Admin.List> result = new List<Banner.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@BannerID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@LoginId", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetBanner", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Banner.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetBannerList. The query was executed :", ex.ToString(), "spu_GetBanner()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public Banner.Admin.Add GetBanner(GetResponse modal)
        {
            Banner.Admin.Add result = new Banner.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@BannerID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@LoginId", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetBanner", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Banner.Admin.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetBanner. The query was executed :", ex.ToString(), "spu_GetBanner()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public PostResponse fnSetBanner(Banner.Admin.Add model)
        {
            PostResponse result = new PostResponse();
            string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetBanner", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@BannerID", SqlDbType.Int).Value = model.BannerID;
                        command.Parameters.Add("@BannerType", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.BannerType);
                        command.Parameters.Add("@Heading", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Heading);
                        command.Parameters.Add("@SubHeading", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.SubHeading);
                        command.Parameters.Add("@InnerPageURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.InnerPageURL);
                        command.Parameters.Add("@DocID", SqlDbType.Int).Value = model.DocID;
                        command.Parameters.Add("@BannerURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.BannerURL);
                        command.Parameters.Add("@Target", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Target.ToString());
                        command.Parameters.Add("@GoToURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.GoToURL);
                        command.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@Priority", SqlDbType.Int).Value = model.Priority??0;
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = model.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = model.IPAddress;

                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.ID = Convert.ToInt64(reader["RET_ID"]);
                                result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (result.StatusCode > 0)
                                {
                                    result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }



        public List<Testimonials.Admin.List> GetTestimonialsList(GetResponse modal)
        {
            List<Testimonials.Admin.List> result = new List<Testimonials.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@TestimonialID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetTestimonial", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Testimonials.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetTestimonialsList. The query was executed :", ex.ToString(), "spu_GetTestimonial()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public Testimonials.Admin.Add GetTestimonials(GetResponse modal)
        {
            Testimonials.Admin.Add result = new Testimonials.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@TestimonialID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetTestimonial", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Testimonials.Admin.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetTestimonials. The query was executed :", ex.ToString(), "spu_GetTestimonial()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public PostResponse fnSetTestimonials(Testimonials.Admin.Add model)
        {
            PostResponse result = new PostResponse();
            string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetTestimonials", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@TestimonialID", SqlDbType.Int).Value = model.TestimonialID;
                        command.Parameters.Add("@Heading", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Heading);
                        command.Parameters.Add("@Author", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Author);
                        command.Parameters.Add("@Description", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Description);
                        command.Parameters.Add("@DocID", SqlDbType.Int).Value = model.DocID;
                        command.Parameters.Add("@ImageURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ImageURL);
                        command.Parameters.Add("@VideoURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.VideoURL);
                        command.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@Priority", SqlDbType.Int).Value = model.Priority ?? 0;
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = model.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = model.IPAddress;

                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.ID = Convert.ToInt64(reader["RET_ID"]);
                                result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (result.StatusCode > 0)
                                {
                                    result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }


        public List<ExternalLinks.Admin.List> GetExternalLinksList(GetResponse modal)
        {
            List<ExternalLinks.Admin.List> result = new List<ExternalLinks.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@LinkID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);

                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetExternalLinks", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<ExternalLinks.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetExternalLinksList. The query was executed :", ex.ToString(), "spu_GetExternalLinks()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public ExternalLinks.Admin.Add GetExternalLinks(GetResponse modal)
        {
            ExternalLinks.Admin.Add result = new ExternalLinks.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@LinkID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetExternalLinks", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<ExternalLinks.Admin.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetExternalLinks. The query was executed :", ex.ToString(), "spu_GetExternalLinks()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public PostResponse fnSetExternalLinks(ExternalLinks.Admin.Add model)
        {
            PostResponse result = new PostResponse();
            string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetExternalLinks", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@LinkID", SqlDbType.Int).Value = model.LinkID;
                        command.Parameters.Add("@DocID", SqlDbType.Int).Value = model.DocID;
                        command.Parameters.Add("@LinkName", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.LinkName);
                        command.Parameters.Add("@LinkURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.LinkURL);
                        command.Parameters.Add("@ImageURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ImageURL);
                        command.Parameters.Add("@Priority", SqlDbType.Int).Value = model.Priority??0;
                        command.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = model.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = model.IPAddress;

                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.ID = Convert.ToInt64(reader["RET_ID"]);
                                result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (result.StatusCode > 0)
                                {
                                    result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }

        public List<FAQ.Admin.List> GetFAQList(GetResponse modal)
        {
            List<FAQ.Admin.List> result = new List<FAQ.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@FAQID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);

                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetFAQ", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<FAQ.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during FAQList. The query was executed :", ex.ToString(), "spu_GetFAQ()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public FAQ.Admin.Add GetFAQ(GetResponse modal)
        {
            FAQ.Admin.Add result = new FAQ.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@FAQID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetFAQ", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<FAQ.Admin.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetFAQ. The query was executed :", ex.ToString(), "spu_GetFAQ()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public PostResponse fnSetFAQ(FAQ.Admin.Add model)
        {
            PostResponse result = new PostResponse();
            string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetFAQ", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@FAQID", SqlDbType.Int).Value = model.FAQID;
                        command.Parameters.Add("@FAQURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.FAQURL);
                        command.Parameters.Add("@Question", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Question);
                        command.Parameters.Add("@Answer", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Answer);
                        command.Parameters.Add("@Priority", SqlDbType.Int).Value = model.Priority ?? 0;
                        command.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = model.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = model.IPAddress;

                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.ID = Convert.ToInt64(reader["RET_ID"]);
                                result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (result.StatusCode > 0)
                                {
                                    result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }



        public List<GalleryGroup.Admin.List> GetGalleryGroupList(GetResponse modal)
        {
            List<GalleryGroup.Admin.List> result = new List<GalleryGroup.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@GalleryGroupID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);

                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetGalleryGroup", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<GalleryGroup.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetGalleryGroupList. The query was executed :", ex.ToString(), "spu_GetGalleryGroup()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public GalleryGroup.Admin.Add GetGalleryGroup(GetResponse modal)
        {
            GalleryGroup.Admin.Add result = new GalleryGroup.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@GalleryGroupID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetGalleryGroup", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<GalleryGroup.Admin.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetGalleryGroup. The query was executed :", ex.ToString(), "spu_GetGalleryGroup()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public PostResponse fnSetGalleryGroup(GalleryGroup.Admin.Add model)
        {
            PostResponse result = new PostResponse();
            string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetGalleryGroup", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@GalleryGroupID", SqlDbType.Int).Value = model.GalleryGroupID;
                        command.Parameters.Add("@GroupDocID", SqlDbType.Int).Value = model.GroupDocID;
                        command.Parameters.Add("@GroupName", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.GroupName);
                        command.Parameters.Add("@Description", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Description);
                        command.Parameters.Add("@ImageURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ImageURL);
                        command.Parameters.Add("@Priority", SqlDbType.Int).Value = model.Priority ?? 0;
                        command.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = model.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = model.IPAddress;

                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.ID = Convert.ToInt64(reader["RET_ID"]);
                                result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (result.StatusCode > 0)
                                {
                                    result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }



        public List<Gallery.Admin.List> GetGalleryList(GetResponse modal)
        {
            List<Gallery.Admin.List> result = new List<Gallery.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@GalleryID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);

                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetGallery", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Gallery.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetGalleryList. The query was executed :", ex.ToString(), "spu_GetGallery()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public Gallery.Admin.Add GetGallery(GetResponse modal)
        {
            Gallery.Admin.Add result = new Gallery.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@GalleryID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetGallery", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Gallery.Admin.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetGallery. The query was executed :", ex.ToString(), "spu_GetGallery()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public PostResponse fnSetGallery(Gallery.Admin.Add model)
        {
            PostResponse result = new PostResponse();
            string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetGallery", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@GalleryID", SqlDbType.Int).Value = model.GalleryID;
                        command.Parameters.Add("@DocID", SqlDbType.Int).Value = model.DocID;
                        command.Parameters.Add("@GalleryGroupID", SqlDbType.Int).Value = model.GalleryGroupID;
                        command.Parameters.Add("@Heading", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Heading);
                        command.Parameters.Add("@Description", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Description);
                        command.Parameters.Add("@ImageURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ImageURL);
                        command.Parameters.Add("@Priority", SqlDbType.Int).Value = model.Priority ?? 0;
                        command.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = model.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = model.IPAddress;

                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.ID = Convert.ToInt64(reader["RET_ID"]);
                                result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (result.StatusCode > 0)
                                {
                                    result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }


        public List<Query.List> GetQueryList(GetResponse modal)
        {
            List<Query.List> result = new List<Query.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@QueryID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@Type", dbType: DbType.String, value:clsCommon.EnsureString(modal.Doctype), direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetQuery", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Query.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetQueryList. The query was executed :", ex.ToString(), "spu_GetQuery()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public List<MetaTags.Admin.List> GetMetaTagsList(GetResponse modal)
        {
            List<MetaTags.Admin.List> result = new List<MetaTags.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@MetaTagsID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetMetaTags", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<MetaTags.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetMetaTagsList. The query was executed :", ex.ToString(), "spu_GetMetaTags()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public MetaTags.Admin.Add GetMetaTags(GetResponse modal)
        {
            MetaTags.Admin.Add result = new MetaTags.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@MetaTagsID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetMetaTags", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<MetaTags.Admin.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetMetaTags. The query was executed :", ex.ToString(), "spu_GetMetaTags()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);
            }
            return result;
        }

        public PostResponse fnSetMetaTags(MetaTags.Admin.Add model)
        {
            PostResponse result = new PostResponse();
            string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetMetaTags", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@MetaTagsID", SqlDbType.Int).Value = model.MetaTagsID;
                        command.Parameters.Add("@PageURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.PageURL);
                        command.Parameters.Add("@MetaName", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.MetaName);
                        command.Parameters.Add("@MetaValue", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.MetaValue);
                        command.Parameters.Add("@Priority", SqlDbType.Int).Value = model.Priority ?? 0;
                        command.Parameters.Add("@IsDefault", SqlDbType.Int).Value = 0;
                        command.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = model.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = model.IPAddress;

                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.ID = Convert.ToInt64(reader["RET_ID"]);
                                result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (result.StatusCode > 0)
                                {
                                    result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }


        public List<Client.Admin.List> GetClientList(GetResponse modal)
        {
            List<Client.Admin.List> result = new List<Client.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@ClientID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetClient", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Client.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetClientList. The query was executed :", ex.ToString(), "spu_GetClient()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public Client.Admin.Add GetClient(GetResponse modal)
        {
            Client.Admin.Add result = new Client.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@ClientID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetClient", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Client.Admin.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetClient. The query was executed :", ex.ToString(), "spu_GetClient()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);
            }
            return result;
        }


        public PostResponse fnSetClient(Client.Admin.Add model)
        {
            PostResponse result = new PostResponse();
            string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetClient", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@ClientID", SqlDbType.Int).Value = model.ClientID;
                        command.Parameters.Add("@FullName", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.FullName);
                        command.Parameters.Add("@Address", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Address);
                        command.Parameters.Add("@CountryID", SqlDbType.Int).Value = model.CountryID ?? 0;
                        command.Parameters.Add("@StateID", SqlDbType.Int).Value = model.StateID ?? 0;
                        command.Parameters.Add("@CityID", SqlDbType.Int).Value = model.CityID ?? 0;
                        command.Parameters.Add("@DocID", SqlDbType.Int).Value = model.DocID;
                        command.Parameters.Add("@ImageURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ImageUrl);
                        command.Parameters.Add("@OfficePhone", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.OfficePhone);
                        command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.MobileNo);
                        command.Parameters.Add("@GSTNo", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.GSTNo);
                        command.Parameters.Add("@Priority", SqlDbType.Int).Value = model.Priority ?? 0;
                        command.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = model.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = model.IPAddress;

                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.ID = Convert.ToInt64(reader["RET_ID"]);
                                result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (result.StatusCode > 0)
                                {
                                    result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }


        public List<MasterAll.Admin.List> GetMasterAllList(GetMasterResponse modal)
        {
            List<MasterAll.Admin.List> result = new List<MasterAll.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@ID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@tablename", dbType: DbType.String, value:clsCommon.EnsureString(modal.TableName), direction: ParameterDirection.Input);
                    param.Add("@groupid", dbType: DbType.Int32, value: modal.GroupID, direction: ParameterDirection.Input);
                    param.Add("@IsActive", dbType: DbType.String, value: clsCommon.EnsureString(modal.IsActive), direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetMasterAll", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<MasterAll.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetMasterAllList. The query was executed :", ex.ToString(), "spu_GetMasterAll()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public MasterAll.Admin.Add GetMasterAll(GetMasterResponse modal)
        {
            MasterAll.Admin.Add result = new MasterAll.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@ID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@tablename", dbType: DbType.String, value: clsCommon.EnsureString(modal.TableName), direction: ParameterDirection.Input);
                    param.Add("@groupid", dbType: DbType.Int32, value: modal.GroupID, direction: ParameterDirection.Input);
                    param.Add("@IsActive", dbType: DbType.String, value: clsCommon.EnsureString(modal.IsActive), direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetMasterAll", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<MasterAll.Admin.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetMasterAll. The query was executed :", ex.ToString(), "spu_GetMasterAll()", "MasterModal", "MasterModal", modal.LoginID, modal.IPAddress);
            }
            return result;
        }

        public PostResponse fnSetMasterAll(MasterAll.Admin.Add model)
        {
            PostResponse result = new PostResponse();
            string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetMasterAll", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@ID", SqlDbType.Int).Value = model.ID;
                        command.Parameters.Add("@table_name", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.table_name);
                        command.Parameters.Add("@field_name", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.field_name);
                        command.Parameters.Add("@field_value", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.field_value);
                        command.Parameters.Add("@group_id", SqlDbType.Int).Value = model.group_id;
                        command.Parameters.Add("@Priority", SqlDbType.Int).Value = model.Priority ?? 0;
                        command.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = model.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = model.IPAddress;

                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.ID = Convert.ToInt64(reader["RET_ID"]);
                                result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (result.StatusCode > 0)
                                {
                                    result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }

    }
}
