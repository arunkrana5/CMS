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
  public  class ProductModal: IProductHelper
    {
        string connectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
        public List<Category.Admin.List> GetCategoryList(GetResponse modal)
        {
            List<Category.Admin.List> result = new List<Category.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(connectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@CategoryID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetCategory", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Category.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetCategoryList. The query was executed :", ex.ToString(), "spu_GetCategory()", "ProductModal", "ProductModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public Category.Admin.Add GetCategory(GetResponse modal)
        {
            Category.Admin.Add result = new Category.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(connectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@CategoryID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetCategory", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Category.Admin.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetCategory. The query was executed :", ex.ToString(), "spu_GetCategory()", "ProductModal", "ProductModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

     

        public PostResponse fnSetCategory(Category.Admin.Add model)
        {
            PostResponse result = new PostResponse();

            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetCategory", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = model.CategoryID;
                        command.Parameters.Add("@CategoryName", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.CategoryName);
                        command.Parameters.Add("@CategoryURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.CategoryURL);
                        command.Parameters.Add("@ImageURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ImageURL);
                        command.Parameters.Add("@Cat_DocID", SqlDbType.Int).Value = model.Cat_DocID;
                        command.Parameters.Add("@CategoryDescription", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.CategoryDescription);
                        command.Parameters.Add("@Location", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Location);
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


        public List<Product.Admin.List> GetProductList(GetResponse modal)
        {
            List<Product.Admin.List> result = new List<Product.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(connectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@ProductID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@LoginID", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetProduct", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Product.Admin.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetProductList. The query was executed :", ex.ToString(), "spu_GetProduct()", "ProductModal", "ProductModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public Product.Admin.Add GetProduct(GetResponse modal)
        {
            Product.Admin.Add result = new Product.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(connectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@ProductID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@LoginID", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetProduct", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Product.Admin.Add>().FirstOrDefault();
                    }
                    
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetProduct. The query was executed :", ex.ToString(), "spu_GetProduct()", "ProductModal", "ProductModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public PostResponse fnSetProduct(Product.Admin.Add model)
        {
            PostResponse result = new PostResponse();
            string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetProduct", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@ProductID", SqlDbType.Int).Value = model.ProductID;
                        command.Parameters.Add("@ProductName", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ProductName);
                        command.Parameters.Add("@ProductURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ProductURL);
                        command.Parameters.Add("@ProductHeading", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ProductHeading);
                        command.Parameters.Add("@ProductShortDescription", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ProductShortDescription);
                        command.Parameters.Add("@OtherDescription", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.OtherDescription);
                        command.Parameters.Add("@ProductDescription", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ProductDescription);
                        command.Parameters.Add("@CategoryIDs", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.CategoryIDs);
                        command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = model.Amount;
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


        public List<ProductImages.Admin.List> GetProductImageList(GetResponse modal)
        {

            List<ProductImages.Admin.List> result = new List<ProductImages.Admin.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(connectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@ProductID", dbType: DbType.Int64, value: modal.AdditionalID, direction: ParameterDirection.Input);
                    param.Add("@ProductImageID", dbType: DbType.Int64, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@ImageType", dbType: DbType.String, value: clsCommon.EnsureString(modal.Doctype), direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetProduct_Image", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<ProductImages.Admin.List>().ToList();
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

        public ProductImages.Admin.Add GetProductImage(GetResponse modal)
        {

            ProductImages.Admin.Add result = new ProductImages.Admin.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(connectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@ProductID", dbType: DbType.Int64, value: modal.AdditionalID, direction: ParameterDirection.Input);
                    param.Add("@ProductImageID", dbType: DbType.Int64, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@ImageType", dbType: DbType.String, value: modal.Doctype, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetProduct_Image", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<ProductImages.Admin.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetProductImage. The query was executed :", ex.ToString(), "spu_GetProduct_Image()", "ProductModal", "ProductModal", modal.LoginID, modal.IPAddress);
            }
            return result;

        }


        public PostResponse fnSetProductImage(ProductImages.Admin.Add model)
        {
            PostResponse result = new PostResponse();
            string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetProductImage", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@ProductImageID", SqlDbType.Int).Value = model.ProductImageID;
                        command.Parameters.Add("@ProductID", SqlDbType.Int).Value = model.ProductID;
                        command.Parameters.Add("@ImageType", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ImageType);
                        command.Parameters.Add("@ImageName", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ImageName);
                        command.Parameters.Add("@ImageURL", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ImageURL);
                        command.Parameters.Add("@ImageDescription", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.ImageDescription);
                        command.Parameters.Add("@Priority", SqlDbType.Int).Value = model.Priority ?? 0;
                        command.Parameters.Add("@IsDefault", SqlDbType.Int).Value = model.IsDefault;
                        command.Parameters.Add("@DocID", SqlDbType.Int).Value = model.DocID;
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
