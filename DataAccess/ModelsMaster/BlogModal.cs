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
    public class BlogModal : IBlogHelper
    {
        string connectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();

        public List<BlogType.Admin.List> GetBlogTypeList(GetResponse modal)
        {
            List<BlogType.Admin.List> result = new List<BlogType.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(connectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@BlogTypeID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetBlogType", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<BlogType.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetBlogTypeList. The query was executed :", ex.ToString(), "spu_GetBlogType()", "BlogModal", "BlogModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }
        public BlogType.Admin.Add GetBlogType(GetResponse modal)
        {
            BlogType.Admin.Add result = new BlogType.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(connectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@BlogTypeID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetBlogType", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<BlogType.Admin.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }

            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetBlogType. The query was executed :", ex.ToString(), "spu_GetBlogType()", "BlogModal", "BlogModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public PostResponse fnSetBlogType(BlogType.Admin.Add model)
        {
            PostResponse result = new PostResponse();
           
            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetBlogType", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@BlogTypeID", SqlDbType.Int).Value = model.BlogTypeID;
                        command.Parameters.Add("@TypeName", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.TypeName);
                        command.Parameters.Add("@TypeURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.TypeURL);
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


        public List<BlogTags.Admin.List> GetBlogTagsList(GetResponse modal)
        {
            List<BlogTags.Admin.List> result = new List<BlogTags.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(connectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@BlogTagID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetBlogTag", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<BlogTags.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetBlogTagsList. The query was executed :", ex.ToString(), "spu_GetBlogTag()", "BlogModal", "BlogModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }
        public BlogTags.Admin.Add GetBlogTags(GetResponse modal)
        {
            BlogTags.Admin.Add result = new BlogTags.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(connectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@BlogTagID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetBlogTag", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<BlogTags.Admin.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }

            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetBlogTags. The query was executed :", ex.ToString(), "spu_GetBlogTag()", "BlogModal", "BlogModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public PostResponse fnSetBlogTags(BlogTags.Admin.Add model)
        {
            PostResponse result = new PostResponse();
           
            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetBlogTag", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@BlogTagID", SqlDbType.Int).Value = model.BlogTagID;
                        command.Parameters.Add("@TagName", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.TagName);
                        command.Parameters.Add("@TagURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.TagURL);
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



        public List<BlogCategory.Admin.List> GetBlogCategoryList(GetResponse modal)
        {
            List<BlogCategory.Admin.List> result = new List<BlogCategory.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(connectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@BlogCategoryID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetBlogCategory", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<BlogCategory.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetBlogCategoryList. The query was executed :", ex.ToString(), "spu_GetBlogCategory()", "BlogModal", "BlogModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }
        public BlogCategory.Admin.Add GetBlogCategory(GetResponse modal)
        {
            BlogCategory.Admin.Add result = new BlogCategory.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(connectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@BlogCategoryID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetBlogCategory", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<BlogCategory.Admin.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }

            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetBlogCategory. The query was executed :", ex.ToString(), "spu_GetBlogCategory()", "BlogModal", "BlogModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public PostResponse fnSetBlogCategory(BlogCategory.Admin.Add model)
        {
            PostResponse result = new PostResponse();
           
            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetBlogCategory", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@BlogCategoryID", SqlDbType.Int).Value = model.BlogCategoryID;
                        command.Parameters.Add("@CategoryDocID", SqlDbType.Int).Value = model.CategoryDocID;
                        command.Parameters.Add("@CategoryName", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.CategoryName);
                        command.Parameters.Add("@CategoryURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.CategoryURL);
                        command.Parameters.Add("@CategoryImage", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.CategoryImage);
                        command.Parameters.Add("@CategoryDesc", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.CategoryDesc);
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



        public List<Blog.Admin.List> GetBlogList(GetResponse modal)
        {
            List<Blog.Admin.List> result = new List<Blog.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(connectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@BlogID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetBlog", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Blog.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetBlogList. The query was executed :", ex.ToString(), "spu_GetBlog()", "BlogModal", "BlogModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }
        public Blog.Admin.Add GetBlog(GetResponse modal)
        {
            Blog.Admin.Add result = new Blog.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(connectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@BlogID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetBlog", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Blog.Admin.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }

            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetBlog. The query was executed :", ex.ToString(), "spu_GetBlog()", "BlogModal", "BlogModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public PostResponse fnSetBlog(Blog.Admin.Add model)
        {
            PostResponse result = new PostResponse();

            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetBlog", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@BlogID", SqlDbType.Int).Value = model.BlogID;
                        command.Parameters.Add("@ListDocID", SqlDbType.Int).Value = model.ListDocID;
                        command.Parameters.Add("@DetailDocID", SqlDbType.Int).Value = model.DetailDocID;
                        command.Parameters.Add("@BlogTypeID", SqlDbType.Int).Value = model.BlogTypeID;
                        command.Parameters.Add("@BlogCategoryIDs", SqlDbType.VarChar).Value = model.BlogCategoryIDs;
                        command.Parameters.Add("@BlogTagIDs", SqlDbType.VarChar).Value = model.BlogTagIDs;
                        command.Parameters.Add("@Heading", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Heading);
                        command.Parameters.Add("@ShortDesc", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ShortDesc);
                        command.Parameters.Add("@Description", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Description);
                        command.Parameters.Add("@BlogURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.BlogURL);
                        command.Parameters.Add("@Target", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Target.ToString());
                        command.Parameters.Add("@ListImageURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ListImageURL);
                        command.Parameters.Add("@DetailImageURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.DetailImageURL);
                        command.Parameters.Add("@AuthorName", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.AuthorName);
                        command.Parameters.Add("@PublishedDate", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.PublishedDate);
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
