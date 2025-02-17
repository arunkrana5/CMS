using Dapper;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Setting
{
    public class Common_SPU
    {
        public static string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();


        public static void LogError(string ErrDescription, string SystemException, string ActiveFunction, string ActiveForm, string ActiveModule,long LoginID,string IPAddress)
        {
            try
            {
                SqlParameter[] oparam = new SqlParameter[7];
                oparam[0] = new SqlParameter("@ErrDescription", clsCommon.EnsureString(ErrDescription));
                oparam[1] = new SqlParameter("@SystemException", clsCommon.EnsureString(SystemException));
                oparam[2] = new SqlParameter("@ActiveFunction", clsCommon.EnsureString(ActiveFunction));
                oparam[3] = new SqlParameter("@ActiveForm", clsCommon.EnsureString(ActiveForm));
                oparam[4] = new SqlParameter("@ActiveModule", clsCommon.EnsureString(ActiveModule));
                oparam[5] = new SqlParameter("@createdby", LoginID);
                oparam[6] = new SqlParameter("@IPAddress", IPAddress);
                DataSet ds = clsDataBaseHelper.ExecuteDataSet("spu_SetErrorLog", oparam);
            }
            catch (Exception ex)
            {
            }

        }
        public static DataSet fnGetConfigSetting(long ConfigID)
        {
            SqlParameter[] oparam = new SqlParameter[1];
            oparam[0] = new SqlParameter("@ConfigID", ConfigID);
            return clsDataBaseHelper.ExecuteDataSet("spu_GetConfigSetting", oparam);
        }

        public static DataSet fnGetLogin(string USERID, string PASSWORD, string SessionID)
        {
            SqlParameter[] oparam = new SqlParameter[3];
            oparam[0] = new SqlParameter("@USERID", USERID);
            oparam[1] = new SqlParameter("@PASSWORD", PASSWORD);
            oparam[2] = new SqlParameter("@SessionID", SessionID);
            return clsDataBaseHelper.ExecuteDataSet("spu_GetLogin", oparam);
        }

        public static PostResponse fnGetCheckURLExist(GetURLExitsResponse Modal)
        {
            PostResponse result = new PostResponse();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_GetCheckURLExist", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@ID", SqlDbType.Int).Value = Modal.ID;
                        command.Parameters.Add("@URL", SqlDbType.VarChar).Value = clsCommon.EnsureString(Modal.URL);
                        command.Parameters.Add("@Doctype", SqlDbType.VarChar).Value = clsCommon.EnsureString(Modal.Doctype);
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
                    Common_SPU.LogError("Error during fnGetCheckURLExist. The query was executed :", ex.ToString(), "spu_GetCheckURLExist()", "Common_SPU", "Common_SPU", Modal.LoginID, Modal.IPAddress);
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }

        public static PostResponse fnGetCheckURLExist_Web(GetURLExitsResponse_Web Modal)
        {
            PostResponse result = new PostResponse();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_GetCheckURLExist_Web", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@Values", SqlDbType.VarChar).Value = Modal.Values??"";
                        command.Parameters.Add("@Doctype", SqlDbType.VarChar).Value = clsCommon.EnsureString(Modal.Doctype);
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
                    Common_SPU.LogError("Error during fnGetCheckURLExist. The query was executed :", ex.ToString(), "spu_GetCheckURLExist()", "Common_SPU", "Common_SPU", Modal.LoginID, Modal.IPAddress);
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }
        public static PostResponse fnGetUpdateColumnResponse(GetUpdateColumnResponse Modal)
        {
            PostResponse result = new PostResponse();
     
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetUpdateColumn_Common", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@ID", SqlDbType.Int).Value = Modal.ID;
                        command.Parameters.Add("@Value", SqlDbType.VarChar).Value = clsCommon.EnsureString(Modal.Value);
                        command.Parameters.Add("@Doctype", SqlDbType.VarChar).Value = clsCommon.EnsureString(Modal.Doctype);
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = Modal.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = Modal.IPAddress;
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
                    Common_SPU.LogError("Error during fnGetUpdateColumnResponse. The query was executed :", ex.ToString(), "spu_SetUpdateColumn_Common()", "Common_SPU", "Common_SPU", Modal.LoginID, Modal.IPAddress);
                    result.StatusCode = -1;
                    result.SuccessMessage = ex.Message.ToString();
                }
            }
            return result;

        }


        public static List<Document.List> GetDocumentList(GetResponse modal)
        {
            List<Document.List> result = new List<Document.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@DocID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@DocTypeID", dbType: DbType.Int32, value: modal.AdditionalID, direction: ParameterDirection.Input);
                    param.Add("@LoginId", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetDocument", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Document.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetDocumentList. The query was executed :", ex.ToString(), "spu_GetDocument()", "Common_SPU", "Common_SPU", modal.LoginID, modal.IPAddress);

            }
            return result;
        }


        public static PostResponse fnSetDocument(Document.Add model)
        {
            PostResponse result = new PostResponse();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetDocument", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@DocID", SqlDbType.Int).Value = model.DocID;
                        command.Parameters.Add("@DocTypeID", SqlDbType.Int).Value = model.DocTypeID;
                        command.Parameters.Add("@Name", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Name);
                        command.Parameters.Add("@content_type", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.content_type);
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


        public static List<DropdownList> GetDropDownList(GetResponse modal)
        {
            List<DropdownList> result = new List<DropdownList>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    param.Add("@Doctype", dbType: DbType.String, value: clsCommon.EnsureString(modal.Doctype), direction: ParameterDirection.Input);
                    param.Add("@ID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    param.Add("@LoginId", dbType: DbType.Int32, value: modal.LoginID, direction: ParameterDirection.Input);
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetDropDownList", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<DropdownList>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetDropDownList. The query was executed :", ex.ToString(), "spu_GetDropDownList()", "Common_SPU", "Common_SPU", modal.LoginID, modal.IPAddress);

            }
            return result;
        }


        public static PostResponse fnSeQuery_ProductDemo(ProductDemo model)
        {
            PostResponse result = new PostResponse();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SeQuery_ProductDemo", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@PDID", SqlDbType.Int).Value = model.PDID??0;
                        command.Parameters.Add("@ProductID", SqlDbType.Int).Value = model.ProductID;
                        command.Parameters.Add("@Name", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Name);
                        command.Parameters.Add("@Email", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Email);
                        command.Parameters.Add("@Mobile", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Phone);
                        command.Parameters.Add("@Query", SqlDbType.VarChar).Value = clsCommon.EnsureString(model.Message);
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



        public static PostResponse fnSetQuery(Query.Add model)
        {
            PostResponse result = new PostResponse();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetQuery", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@QueryID", SqlDbType.Int).Value = model.QueryID??0;
                        command.Parameters.Add("@Type", SqlDbType.VarChar).Value = model.Type??"";
                        command.Parameters.Add("@Name", SqlDbType.VarChar).Value = model.Name ?? "";
                        command.Parameters.Add("@Email", SqlDbType.VarChar).Value = model.Email ?? "";
                        command.Parameters.Add("@Subject", SqlDbType.VarChar).Value = model.Subject ?? "";
                        command.Parameters.Add("@Mobile", SqlDbType.VarChar).Value = model.Mobile ?? "";
                        command.Parameters.Add("@Query", SqlDbType.VarChar).Value = model.Query ?? "";
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
