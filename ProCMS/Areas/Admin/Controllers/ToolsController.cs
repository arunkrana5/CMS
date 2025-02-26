﻿using DataAccess.Models;
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
    public class ToolsController : Controller
    {
        IToolHelper Tools;
        long LoginID = 0;
        string IPAddress = "";
        GetResponse getResponse;
        public ToolsController()
        {
            getResponse = new GetResponse();
            Tools = new ToolsModal();
            long.TryParse(clsApplicationSetting.GetSessionValue("LoginID"), out LoginID);
            IPAddress = clsApplicationSetting.GetIPAddress();
            getResponse.IPAddress = IPAddress;
            getResponse.LoginID = LoginID;

        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult ErrorLogList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<ErrorLog> Modal = new List<ErrorLog>();
            Modal = Tools.ErrorLogList(getResponse);
            return View(Modal);

        }

        [HttpPost]
        public ActionResult ErrorLogList(string src, FormCollection Form, string Command)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];

            string SQL = "", ChkAll = "", Chksingle = "";
            long SaveID = 0;
            try
            {
                if (Command == "Delete")
                {
                    if (Form.GetValue("ChkAll") != null)
                    {
                        ChkAll = Form.GetValue("ChkAll").AttemptedValue;
                    }
                    if (Form.GetValue("Chksingle") != null)
                    {
                        Chksingle = Form.GetValue("Chksingle").AttemptedValue;
                    }

                    if (ChkAll == "All")
                    {
                        SQL = "truncate table ErrorLog";
                        clsDataBaseHelper.ExecuteNonQuery(SQL);
                        SaveID = 1;
                    }
                    else if (!string.IsNullOrEmpty(Chksingle))
                    {
                        SQL = "delete from ErrorLog where ID in (" + Chksingle + ")";
                        SaveID = clsDataBaseHelper.ExecuteNonQuery(SQL);
                    }
                }

                if (SaveID > 0)
                {
                    TempData["Success"] = "Y";
                    TempData["SuccessMsg"] = "Data Deleted Successfully";

                }
                else
                {
                    TempData["Success"] = "N";
                    TempData["SuccessMsg"] = "Data Can not be Delete";
                }
            }
            catch (Exception Ex)
            {
                Common_SPU.LogError("Error during ErrorLogList. The query was executed :", Ex.ToString(), "ToolsController()", "ToolsController", "ToolsController", LoginID, IPAddress);

               
            }
            return RedirectToAction("ErrorLogList", new { src = clsCommon.Encrypt(ViewBag.MenuID + "*" + "/Admin/Tools/ErrorLogList") });

        }

        public ActionResult RoleList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];

            List<AdminUser.Role.List> Modal = new List<AdminUser.Role.List>();
            Modal = Tools.GetRolesList(getResponse);
            return View(Modal);
        }


        public ActionResult _AddRole(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.RoleID = GetQueryString[2];
            AdminUser.Role.Add Modal = new AdminUser.Role.Add();
            long ID = 0;
            long.TryParse(ViewBag.RoleID, out ID);
            if (ID > 0)
            {
                getResponse.ID = ID;
                Modal = Tools.GetRoles(getResponse);
            }

            return PartialView(Modal);

        }
        [HttpPost]
        public ActionResult _AddRole(string src, AdminUser.Role.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.RoleID = GetQueryString[2];
            long RoleID = 0;
            long.TryParse(ViewBag.RoleID, out RoleID);
            Result.SuccessMessage = "Role Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Result = Tools.fnSetUserRole(Modal);

            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Tools/RoleList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Tools/RoleList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RoleMenuManagement(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.RoleID = GetQueryString[2];
            int ID = 0;
            int.TryParse(ViewBag.RoleID, out ID);
            List<AdminModule> Modal = new List<AdminModule>();
            if (ID > 0)
            {
                getResponse.ID = ID;
                Modal = Tools.GetModuleListWithMenu(getResponse);
            }
            return View(Modal);
        }

        private ArrayList ChildMenuManagement(FormCollection Form, List<AdminMenu> ChildMenuList)
        {
            string SQL = "";
            ArrayList ArStr = new ArrayList();
            foreach (AdminMenu Mitem2 in ChildMenuList)
            {
                bool read = false, write = false, modify = false, delete = false, export = false;
                if (Form.GetValue("Read_" + Mitem2.MenuID) != null)
                {
                    read = true;
                }

                SQL = "update Menu_Role_Tran set r='" + read + "',modifiedat=getdate(), modifiedby=" + LoginID + " where TranID=" + Mitem2.TranID;
                ArStr.Add(SQL);

                if (Form.GetValue("Write_" + Mitem2.MenuID) != null)
                {
                    write = true;
                }

                SQL = "update Menu_Role_Tran set w='" + write + "',modifiedat=getdate(), modifiedby=" + LoginID + " where TranID=" + Mitem2.TranID;
                ArStr.Add(SQL);

                if (Form.GetValue("Modify_" + Mitem2.MenuID) != null)
                {
                    modify = true;
                }

                SQL = "update Menu_Role_Tran set m='" + modify + "',modifiedat=getdate(), modifiedby=" + LoginID + " where TranID=" + Mitem2.TranID;
                ArStr.Add(SQL);

                if (Form.GetValue("Delete_" + Mitem2.MenuID) != null)
                {
                    delete = true;
                }

                SQL = "update Menu_Role_Tran set d='" + delete + "',modifiedat=getdate(), modifiedby=" + LoginID + " where TranID=" + Mitem2.TranID;
                ArStr.Add(SQL);

                if (Form.GetValue("Export_" + Mitem2.MenuID) != null)
                {
                    export = true;
                }

                SQL = "update Menu_Role_Tran set e='" + export + "',modifiedat=getdate(), modifiedby=" + LoginID + " where TranID=" + Mitem2.TranID;
                ArStr.Add(SQL);


                if (Mitem2.IsChild == "Y")
                {
                    ChildMenuManagement(Form, Mitem2.ChildMenuList);
                }

            }
            return ArStr;
        }

        [HttpPost]
        public ActionResult RoleMenuManagement(string src, FormCollection Form, string Command)
        {
            int SaveID = 0;
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.RoleID = GetQueryString[2];
            ViewBag.RoleName = GetQueryString[3];
            ArrayList ArStr = new ArrayList();
            long LoginID = 0;
            long.TryParse(clsApplicationSetting.GetSessionValue("LoginID"), out LoginID);
            string SQL = "";
            try
            {
                if (Command == "Update")
                {
                    int ID = 0;
                    int.TryParse(ViewBag.RoleID, out ID);

                    List<AdminModule> Modal = new List<AdminModule>();
                    if (ID > 0)
                    {
                        getResponse.ID = ID;
                        Modal = Tools.GetModuleListWithMenu(getResponse);
                    }
                    foreach (AdminModule item in Modal)
                    {
                        foreach (AdminMenu Mitem in item.MainMenuList)
                        {
                            bool read = false, write = false, modify = false, delete = false, export = false;
                            if (Form.GetValue("Read_" + Mitem.MenuID) != null)
                            {
                                read = true;
                            }

                            SQL = "update Menu_Role_Tran set r='" + read + "', modifiedat=getdate(), modifiedby=" + LoginID + " where TranID=" + Mitem.TranID;
                            ArStr.Add(SQL);

                            if (Form.GetValue("Write_" + Mitem.MenuID) != null)
                            {
                                write = true;
                            }

                            SQL = "update Menu_Role_Tran set w='" + write + "', modifiedat=getdate(), modifiedby=" + LoginID + " where TranID=" + Mitem.TranID;
                            ArStr.Add(SQL);

                            if (Form.GetValue("Modify_" + Mitem.MenuID) != null)
                            {
                                modify = true;
                            }

                            SQL = "update Menu_Role_Tran set m='" + modify + "',modifiedat=getdate(), modifiedby=" + LoginID + " where TranID=" + Mitem.TranID;
                            ArStr.Add(SQL);

                            if (Form.GetValue("Delete_" + Mitem.MenuID) != null)
                            {
                                delete = true;
                            }

                            SQL = "update Menu_Role_Tran set d='" + delete + "',modifiedat=getdate(), modifiedby=" + LoginID + " where TranID=" + Mitem.TranID;
                            ArStr.Add(SQL);

                            if (Form.GetValue("Export_" + Mitem.MenuID) != null)
                            {
                                export = true;
                            }

                            SQL = "update Menu_Role_Tran set e='" + export + "',modifiedat=getdate(), modifiedby=" + LoginID + " where TranID=" + Mitem.TranID;
                            ArStr.Add(SQL);


                            if (Mitem.IsChild == "Y")
                            {

                                var ChildList = ChildMenuManagement(Form, Mitem.ChildMenuList);
                                if (ChildList != null)
                                {
                                    foreach (var myitem in ChildList)
                                    {
                                        ArStr.Add(myitem);
                                    }
                                }
                            }
                        }
                    }

                    SaveID = clsDataBaseHelper.executeArrayOfSql(ArStr);


                }
                else if (Command == "Sync")
                {
                    clsDataBaseHelper.ExecuteNonQuery("exec spu_Update_Menu_Role_Tran");
                    TempData["Success"] = "Y";
                    TempData["SuccessMsg"] = "All Menu Synced Successfully";
                    return RedirectToAction("RoleMenuManagement", new { src = clsCommon.Encrypt(ViewBag.MenuID + "*" + "/Admin/Tools/RoleMenuManagement" + "*" + ViewBag.RoleID + "*" + ViewBag.RoleName) });
                }
                else if (Command == "JSON")
                {
                    clsApplicationSetting.CreateMenuJSon();
                    TempData["Success"] = "Y";
                    TempData["SuccessMsg"] = "Menu JSON Updated Successfully";
                    return RedirectToAction("RoleMenuManagement", new { src = clsCommon.Encrypt(ViewBag.MenuID + "*" + "/Admin/Tools/RoleMenuManagement" + "*" + ViewBag.RoleID + "*" + ViewBag.RoleName) });

                }
                if (SaveID > 0)
                {
                    TempData["Success"] = "Y";
                    TempData["SuccessMsg"] = "Setting Saved Successfully";
                }
                else
                {
                    TempData["Success"] = "N";
                    TempData["SuccessMsg"] = "Setting is not Saved";
                }
            }
            catch (Exception ex)
            {
                Common_SPU.LogError("Error during GetModuleListWithMenu. The query was executed :", ex.ToString(), "spu_GetModuleListRoleWise()", "HomeModal", "HomeModal", LoginID, IPAddress);
            }
            return RedirectToAction("RoleList", new { src = clsCommon.Encrypt(ViewBag.MenuID + "*" + "/Admin/Tools/RoleList") });

        }


        public ActionResult EmailTemplateList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<EmailTemplate.List> Modal = new List<EmailTemplate.List>();
            Modal = Tools.GetEmailTemplateList(getResponse); 
            return View(Modal);
        }
        public ActionResult EmailTemplateAdd(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.TemplateID = GetQueryString[2];

            long ID = 0;
            long.TryParse(ViewBag.TemplateID, out ID);
            EmailTemplate.Add Modal = new EmailTemplate.Add();
            if (ID > 0)
            {
                getResponse.ID = ID;
                Modal = Tools.GetEmailTemplate(getResponse);
            }
            return View(Modal);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EmailTemplateAdd(string src, EmailTemplate.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.TemplateID = GetQueryString[2];
            long TemplateID = 0;
            long.TryParse(ViewBag.TemplateID, out TemplateID);
            Result.SuccessMessage = "Email Template Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Result = Tools.fnSetEmailTemplate(Modal);

            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Tools/EmailTemplateList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Tools/EmailTemplateList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }



        public ActionResult UserList(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            List<Users.List> Modal = new List<Users.List>();
            Modal = Tools.GetUsersList(getResponse);
            return View(Modal);
        }

        public ActionResult _AddUser(string src)
        {
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.ID = GetQueryString[2];
            Users.Add Modal = new Users.Add();
            long ID = 0;
            long.TryParse(ViewBag.ID, out ID);
            if (ID > 0)
            {
                getResponse.ID = ID;
                Modal = Tools.GetUsers(getResponse);
            }
            GetResponse getRoleResponses = new GetResponse();
            getRoleResponses.LoginID = LoginID;
            getRoleResponses.IPAddress = IPAddress;
            ViewBag.RoleList = Tools.GetRolesList(getRoleResponses).Where(x => x.IsActive).ToList();

            return PartialView(Modal);

        }

        [HttpPost]
        public ActionResult _AddUser(string src, Users.Add Modal, string Command)
        {
            PostResponse Result = new PostResponse();
            ViewBag.src = src;
            string[] GetQueryString = clsApplicationSetting.DecryptQueryString(src);
            ViewBag.GetQueryString = GetQueryString;
            ViewBag.MenuID = GetQueryString[0];
            ViewBag.ID = GetQueryString[2];
            long ID = 0;
            long.TryParse(ViewBag.ID, out ID);
            Result.SuccessMessage = "User Can't Update";
            if (ModelState.IsValid)
            {
                Modal.LoginID = LoginID;
                Modal.IPAddress = IPAddress;
                Result = Tools.fnSetUsers(Modal);

            }
            if (Result.Status)
            {
                Result.RedirectURL = "/Admin/Tools/UserList?src=" + clsCommon.Encrypt(ViewBag.MenuID.ToString() + "*/Admin/Tools/UserList");
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }


    }
}