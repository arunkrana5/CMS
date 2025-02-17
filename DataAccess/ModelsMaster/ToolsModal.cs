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
    public class ToolsModal: IToolHelper
    {
        string ConnectionStrings = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString.ToString();

        public List<AdminMenu> GetAdminMenuList(GetResponse modal)
        {
            List<AdminMenu> result = new List<AdminMenu>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetMenu_Admin", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<AdminMenu>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetAdminMenuList. The query was executed :", ex.ToString(), "spu_GetMenu_Admin()", "HomeModal", "HomeModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }
        public List<ErrorLog> ErrorLogList(GetResponse modal)
        {
            List<ErrorLog> result = new List<ErrorLog>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    DBContext.Open();
                    using (var reader = DBContext.QueryMultiple("spu_GetErrorLog", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<ErrorLog>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during ErrorLogList. The query was executed :", ex.ToString(), "spu_GetErrorLog()", "ToolsModal", "ToolsModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public List<AdminUser.Role.List> GetRolesList(GetResponse modal)
        {
            List<AdminUser.Role.List> result = new List<AdminUser.Role.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    DBContext.Open();
                    param.Add("@ID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    using (var reader = DBContext.QueryMultiple("spu_GetRoles", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<AdminUser.Role.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetRolesList. The query was executed :", ex.ToString(), "spu_GetRoles()", "ToolsModal", "ToolsModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }


        public AdminUser.Role.Add GetRoles(GetResponse modal)
        {
            AdminUser.Role.Add result = new AdminUser.Role.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    DBContext.Open();
                    param.Add("@ID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    using (var reader = DBContext.QueryMultiple("spu_GetRoles", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<AdminUser.Role.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetRoles. The query was executed :", ex.ToString(), "spu_GetRoles()", "HomeModal", "HomeModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }
        public List<AdminModule> GetModuleListWithMenu(GetResponse modal)
        {
            List<AdminModule> result = new List<AdminModule>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    DBContext.Open();
                    param.Add("@Roleid", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    using (var reader = DBContext.QueryMultiple("spu_GetModuleListRoleWise", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<AdminModule>().ToList();
                    }
                    DBContext.Close();
                }
                if (result != null)
                {
                    foreach (AdminModule item in result)
                    {
                        item.MainMenuList = GetLoginMenuListWithDetails(item.ModuleID, modal.ID, modal.LoginID, modal.IPAddress);
                    }
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetModuleListWithMenu. The query was executed :", ex.ToString(), "spu_GetModuleListRoleWise()", "HomeModal", "HomeModal", modal.LoginID, modal.IPAddress);
            }
            return result;
        }

        private List<AdminMenu> GetLoginMenuListWithDetails(long ModuleID, long RoleID,long LoginID,string IPAddress, long ParentMenuID = 0)
        {
            List<AdminMenu> List = new List<AdminMenu>();
            try
            {
                GetResponse getResponse = new GetResponse();
                getResponse.LoginID = LoginID;
                getResponse.IPAddress = IPAddress;
                List = GetAdminMenuList(getResponse).Where(x => x.ModuleID == ModuleID && x.RoleID == RoleID && x.ParentMenuID == ParentMenuID).ToList();
                
                if(List!=null && List.Count>0)
                {
                    foreach (var item in List.Where(x=>x.IsChild=="Y").ToList())
                    {
                        item.ChildMenuList = GetLoginMenuListWithDetails(ModuleID, RoleID, LoginID, IPAddress, item.MenuID);
                    }

                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetModuleListWithMenu. The query was executed :", ex.ToString(), "spu_GetModuleListRoleWise()", "HomeModal", "HomeModal", 0, "");
            }
            return List;
        }



        public PostResponse fnSetUserRole(AdminUser.Role.Add modal)
        {
            PostResponse Result = new PostResponse();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetUserRole", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("ID", SqlDbType.Int).Value = modal.ID;
                        command.Parameters.Add("@Role_Name", SqlDbType.VarChar).Value = clsCommon.EnsureString(modal.role_name);
                        command.Parameters.Add("@description", SqlDbType.VarChar).Value = clsCommon.EnsureString(modal.description);
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = modal.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = modal.IPAddress;
                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Result.ID = Convert.ToInt64(reader["RET_ID"]);
                                Result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                Result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (Result.StatusCode > 0)
                                {
                                    Result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    Result.StatusCode = -1;
                    Result.SuccessMessage = ex.Message.ToString();
                }
            }
            return Result;
        }


        public List<EmailTemplate.List> GetEmailTemplateList(GetResponse modal)
        {
            List<EmailTemplate.List> result = new List<EmailTemplate.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    DBContext.Open();
                    param.Add("@ID", dbType: DbType.Int32, value: 0, direction: ParameterDirection.Input);
                    using (var reader = DBContext.QueryMultiple("spu_GetEmailTemplate", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<EmailTemplate.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetEmailTemplateList. The query was executed :", ex.ToString(), "spu_GetEmailTemplate()", "HomeModal", "HomeModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }
        public EmailTemplate.Add GetEmailTemplate(GetResponse modal)
        {
            EmailTemplate.Add result = new EmailTemplate.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    DBContext.Open();
                    param.Add("@ID", dbType: DbType.Int32, value: 0, direction: ParameterDirection.Input);
                    using (var reader = DBContext.QueryMultiple("spu_GetEmailTemplate", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<EmailTemplate.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetEmailTemplate. The query was executed :", ex.ToString(), "spu_GetEmailTemplate()", "ToolsModal", "ToolsModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public PostResponse fnSetEmailTemplate(EmailTemplate.Add modal)
        {
            PostResponse Result = new PostResponse();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetEmailTemplate", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("ID", SqlDbType.Int).Value = modal.ID;
                        command.Parameters.Add("@TemplateName", SqlDbType.VarChar).Value = clsCommon.EnsureString(modal.TemplateName);
                        command.Parameters.Add("@Body", SqlDbType.VarChar).Value = clsCommon.EnsureString(modal.Body);
                        command.Parameters.Add("@Subject", SqlDbType.VarChar).Value = clsCommon.EnsureString(modal.Subject);
                        command.Parameters.Add("@CCMail", SqlDbType.VarChar).Value = clsCommon.EnsureString(modal.CCMail);
                        command.Parameters.Add("@BCCMail", SqlDbType.VarChar).Value = clsCommon.EnsureString(modal.BCCMail);
                        command.Parameters.Add("@SMSBody", SqlDbType.VarChar).Value = clsCommon.EnsureString(modal.SMSBody);
                        command.Parameters.Add("@Repository", SqlDbType.VarChar).Value = clsCommon.EnsureString(modal.Repository);
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = modal.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = modal.IPAddress;
                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Result.ID = Convert.ToInt64(reader["RET_ID"]);
                                Result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                Result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (Result.StatusCode > 0)
                                {
                                    Result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    Result.StatusCode = -1;
                    Result.SuccessMessage = ex.Message.ToString();
                }
            }
            return Result;
        }


        public List<Users.List> GetUsersList(GetResponse modal)
        {
            List<Users.List> result = new List<Users.List>();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    DBContext.Open();
                    param.Add("@ID", dbType: DbType.Int32, value: 0, direction: ParameterDirection.Input);
                    using (var reader = DBContext.QueryMultiple("spu_GetUser", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Users.List>().ToList();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetusersList. The query was executed :", ex.ToString(), "spu_GetUser()", "ToolsModal", "ToolsModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }
        public Users.Add GetUsers(GetResponse modal)
        {
            Users.Add result = new Users.Add();
            try
            {
                using (IDbConnection DBContext = new SqlConnection(ConnectionStrings))
                {
                    var param = new DynamicParameters();
                    DBContext.Open();
                    param.Add("@ID", dbType: DbType.Int32, value: modal.ID, direction: ParameterDirection.Input);
                    using (var reader = DBContext.QueryMultiple("spu_GetUser", param: param, commandType: CommandType.StoredProcedure))
                    {
                        result = reader.Read<Users.Add>().FirstOrDefault();
                    }
                    DBContext.Close();
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetUsers. The query was executed :", ex.ToString(), "spu_GetUser()", "ToolsModal", "ToolsModal", modal.LoginID, modal.IPAddress);

            }
            return result;
        }

        public PostResponse fnSetUsers(Users.Add modal)
        {
            PostResponse Result = new PostResponse();
            using (SqlConnection con = new SqlConnection(ConnectionStrings))
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("spu_SetUsers", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("ID", SqlDbType.Int).Value = modal.ID;
                        command.Parameters.Add("@userID", SqlDbType.VarChar).Value = clsCommon.EnsureString(modal.UserID);
                        command.Parameters.Add("@password", SqlDbType.VarChar).Value = clsCommon.Encrypt(modal.Password);
                        command.Parameters.Add("@Name", SqlDbType.VarChar).Value = clsCommon.EnsureString(modal.Name);
                        command.Parameters.Add("@Email", SqlDbType.VarChar).Value = clsCommon.EnsureString(modal.email);
                        command.Parameters.Add("@Phone", SqlDbType.VarChar).Value = clsCommon.EnsureString(modal.Phone);
                        command.Parameters.Add("@Description", SqlDbType.VarChar).Value = clsCommon.EnsureString(modal.Description);
                        command.Parameters.Add("@roleid", SqlDbType.Int).Value = modal.RoleID;
                        command.Parameters.Add("@createdby", SqlDbType.Int).Value = modal.LoginID;
                        command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = modal.IPAddress;
                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Result.ID = Convert.ToInt64(reader["RET_ID"]);
                                Result.StatusCode = Convert.ToInt32(reader["COMMANDSTATUS"]);
                                Result.SuccessMessage = reader["COMMANDMESSAGE"].ToString();
                                if (Result.StatusCode > 0)
                                {
                                    Result.Status = true;
                                }
                            }
                        }

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    Result.StatusCode = -1;
                    Result.SuccessMessage = ex.Message.ToString();
                }
            }
            return Result;
        }

    }
}
