using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using DataAccess.Models;
using DataAccess.Setting;
using DataAccess.ModelsMaster;
using DataAccess.ModelsMasterHelper;

public class clsApplicationSetting
{
    public static string DesignKey = clsApplicationSetting.GetWebConfigValue("DesignKey");



    public static string GetWebConfigValue(string Key)
    {
        string functionReturnValue = null;
        functionReturnValue = "";
        try
        {
            return ConfigurationManager.AppSettings[Key].ToString();
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("Key " + Key + "<br>" + ex.Message);
            HttpContext.Current.Response.End();
        }
        return functionReturnValue;

    }

    #region Session And Cookies By Ravi
    //Maintain Session And Cookies By Ravi Start 
    public static bool SetSessionValue(string SessionName, string SessionValue)
    {
        try
        {
            string CookiesExpireTime = GetWebConfigValue("CookiesExpireTime");
            if (!string.IsNullOrEmpty(CookiesExpireTime))
            {
                HttpContext.Current.Session[SessionName] = SessionValue;
                HttpContext.Current.Session.Timeout = Convert.ToInt32(CookiesExpireTime);

            }
            else
            {
                HttpContext.Current.Session[SessionName] = SessionValue;
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }

    public static string GetSessionValue(string sessionname)
    {
        string SessionValue = null;
        if ((HttpContext.Current.Session[sessionname] != null))
        {
            SessionValue = HttpContext.Current.Session[sessionname].ToString();
        }
        return SessionValue;

    }

    public static bool SetCookiesValue(string CookiesName, string CookiesValue)
    {
        HttpCookie Cook = default(HttpCookie);

        try
        {
            string CookiesExpireTime = GetWebConfigValue("CookiesExpireTime");
            Cook = new HttpCookie(CookiesName, clsCommon.Encrypt(CookiesValue));
            if (!string.IsNullOrEmpty(CookiesExpireTime))
            {
                Cook.Expires = DateTime.Now.AddMinutes(Convert.ToInt32(CookiesExpireTime));
            }
            else
            {
                Cook.Expires = DateTime.Now.AddDays(1);
            }
            HttpContext.Current.Response.Cookies.Add(Cook);
            HttpContext.Current.Session.Add(CookiesName, CookiesValue);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }
    public static string GetCookiesValue(string sessionname)
    {
        string SessionValue = null;
        if ((HttpContext.Current.Request.Cookies[sessionname] != null))
        {
            SessionValue = clsCommon.Decrypt(HttpContext.Current.Request.Cookies[sessionname].Value.ToString());
        }
        return SessionValue;

    }


    public static bool ClearCookiesValue(string SessionName)
    {
        try
        {
            if (HttpContext.Current.Request.Cookies[SessionName] != null)
            {
                HttpContext.Current.Response.Cookies[SessionName].Expires = DateTime.Now.AddDays(-1);
            }

            HttpContext.Current.Session[SessionName] = "";

        }
        catch (Exception ex)
        {
        }
        return true;
    }

    public static bool ClearSessionValues()
    {

        try
        {
            if (HttpContext.Current != null)
            {
                int cookieCount = HttpContext.Current.Request.Cookies.Count;
                for (var i = 0; i < cookieCount; i++)
                {
                    var cookie = HttpContext.Current.Request.Cookies[i];
                    if (cookie != null)
                    {
                        var expiredCookie = new HttpCookie(cookie.Name)
                        {
                            Expires = DateTime.Now.AddDays(-1),
                            Domain = cookie.Domain
                        };
                        HttpContext.Current.Response.Cookies.Add(expiredCookie); // overwrite it
                    }
                }

                // clear cookies server side
                HttpContext.Current.Request.Cookies.Clear();

                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.RemoveAll();
                HttpContext.Current.Session.Abandon();


            }

        }
        catch (Exception ex)
        {
        }
        return true;

    }

    public static bool UpdateCookieValue(string CookieName, string Value)
    {
        CookieName = CookieName.ToUpper();
        HttpCookie cook = new HttpCookie(CookieName, Value);
        HttpContext.Current.Response.Cookies.Add(cook);
        HttpContext.Current.Session[CookieName] = HttpContext.Current.Request.Cookies[CookieName].Value;
        return true;
    }

    public static bool IsSessionExpired(string SessionText)
    {

        bool IsExpired = false;

        //Or HttpContext.Current.Session[SessionText] Is Nothing Then
        if (HttpContext.Current.Session[SessionText] == null || HttpContext.Current.Session[SessionText].ToString() == "")
        {
            IsExpired = true;
        }

        return IsExpired;

    }
    public static bool IsCookiesExpired(string SessionText)
    {

        bool IsExpired = false;

        //Or HttpContext.Current.Session[SessionText] Is Nothing Then
        if (HttpContext.Current.Request.Cookies[SessionText] == null || HttpContext.Current.Request.Cookies[SessionText].ToString() == "")
        {
            IsExpired = true;
        }

        return IsExpired;

    }
    // Maintain Session And Cookies By Ravi End 
    #endregion



    public static string GetPhysicalPath(string pathFor = "")
    {
        string functionReturnValue = "";
        if (pathFor.ToLower() == "images")
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/assets/design" + DesignKey + "/images")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/assets/design" + DesignKey + "/images"));
            }
            functionReturnValue = HttpContext.Current.Server.MapPath("/assets/design" + DesignKey + "/images");
        }
        else if (pathFor.ToLower() == "json")
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Attachments/UserDetails/Jsondata")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Attachments/UserDetails/Jsondata"));
            }
            functionReturnValue = HttpContext.Current.Server.MapPath("/Attachments/UserDetails/Jsondata");
        }
        else
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Attachments")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Attachments"));
            }
            functionReturnValue = HttpContext.Current.Server.MapPath("/Attachments");
        }
        return functionReturnValue;
    }



    public static string AllowedFileSize(string Type)
    {
        double byteCount = 0;
        if (Type == "Image")
        {
            double.TryParse(clsApplicationSetting.GetWebConfigValue("AllowedImage_Size"), out byteCount);
        }
        else
        {
            double.TryParse(clsApplicationSetting.GetWebConfigValue("AllowedFile_Size"), out byteCount);
        }

        return GetFileSize(byteCount);
    }

    public static string GetFileSize(double byteCount)
    {
        string size = "0 Bytes";
        if (byteCount >= 1073741824.0)
            size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
        else if (byteCount >= 1048576.0)
            size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
        else if (byteCount >= 1024.0)
            size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
        else if (byteCount > 0 && byteCount < 1024.0)
            size = byteCount.ToString() + " Bytes";

        return size;
    }

    public static FileResponse ValidateFile(HttpPostedFileBase File)
    {
        FileResponse obj = new FileResponse();
        try
        {
            obj.FileType = File.ContentType;
            obj.InputStream = File.InputStream;
            obj.FileName = System.IO.Path.GetFileName(File.FileName);
            obj.FileLength = File.ContentLength;
            obj.FileExt = System.IO.Path.GetExtension(File.FileName).ToLower();
            obj.IsValid = true;
            string AIMGExt = GetWebConfigValue("AllowedImage_Ext").ToLower();
            string AFExt = GetWebConfigValue("AllowedFile_Ext").ToLower();
            long AIMGSize, AFSize = 0, AMaxmiumSize = 0;
            long.TryParse(GetWebConfigValue("AllowedImage_Size"), out AIMGSize);
            long.TryParse(GetWebConfigValue("AllowedFile_Size"), out AFSize);
            AMaxmiumSize = (AIMGSize > AFSize ? AIMGSize : AFSize);

            obj.ReadAbleFileSize = GetFileSize(obj.FileLength);
            BinaryReader chkBinary = new BinaryReader(obj.InputStream);
            Byte[] chkbytes = chkBinary.ReadBytes(0x10);
            string data_as_hex = BitConverter.ToString(chkbytes);
            string magicCheck = data_as_hex.Substring(0, 8);
            Dictionary<string, string> MDict = new Dictionary<string, string>();
            MDict = GetMagicNumnberDictionary();
            if (MDict != null && MDict.Count > 0)
            {
                if ((".txt").Contains(obj.FileExt))
                {
                    obj.IsValid = true;
                }
                else if (MDict.ContainsKey(obj.FileExt))
                {
                    if ((".jpg,.jpeg,.png").Contains(obj.FileExt))
                    {
                        if (MDict[".jpg"].Substring(0, 8).Replace(" ", "-") != magicCheck && MDict[".jpeg"].Substring(0, 8).Replace(" ", "-") != magicCheck && MDict[".png"].Substring(0, 8).Replace(" ", "-") != magicCheck)
                        {
                            obj.IsValid = false;
                            var myKey = MDict.FirstOrDefault(x => x.Value.Contains(magicCheck)).Key;
                            obj.Message = "Please Upload Valid File with Original extension," + (!string.IsNullOrEmpty(myKey) ? "It seems it is " + myKey + " file" : "");
                        }
                    }
                    else if (MDict[obj.FileExt].Substring(0, 8).Replace(" ", "-") != magicCheck)
                    {
                        obj.IsValid = false;
                        var myKey = MDict.FirstOrDefault(x => x.Value.Contains(magicCheck)).Key;
                        obj.Message = "Please Upload Valid File with Original extension," + (!string.IsNullOrEmpty(myKey) ? "It seems it is " + myKey + " file" : "");
                    }
                }
                else if (obj.IsValid)
                {
                    if (!(AIMGExt.Replace("|", ",")).Contains(obj.FileExt))
                    {
                        obj.IsValid = false;
                        obj.Message = "Can't Upload Image Extention Must Be Matched";

                    }
                    else if (!(AFExt.Replace("|", ",")).Contains(obj.FileExt))
                    {
                        obj.IsValid = false;
                        obj.Message = "Can't Upload File Extention Must Be Matched";

                    }
                    if (obj.IsValid)
                    {
                        if ((AIMGExt.Replace("|", ",")).Contains(obj.FileExt) && obj.FileLength > AIMGSize)
                        {
                            obj.IsValid = false;
                            obj.Message = "Can't Upload Image Size Must Be Matched";

                        }
                        else if ((AFExt.Replace("|", ",")).Contains(obj.FileExt) && obj.FileLength > AFSize)
                        {
                            obj.IsValid = false;
                            obj.Message = "Can't Upload File Size Must Be Matched";

                        }
                    }
                }
            }
            else
            {
                obj.IsValid = false;
                obj.Message = "Please Add Magic Number Into .Txt File";
            }
        }
        catch (Exception ex)
        {
            //ClsCommon.LogError("Error during Problem in ValidateFileWithMagicNumber. The query was executed :", ex.ToString(), "", "ClsApplicationSetting", "ClsApplicationSetting", "");
        }
        return obj;
    }

    public static Dictionary<string, string> GetMagicNumnberDictionary()
    {
        Dictionary<string, string> MagicNumnberDictionary = new Dictionary<string, string>();
        try
        {
            string GetPath = HttpContext.Current.Server.MapPath("/Attachments/UserDetails/temp");
            if (!Directory.Exists(GetPath))
            {
                Directory.CreateDirectory(GetPath);
            }
            if (System.IO.File.Exists(GetPath + "/MagicNumber.txt"))
            {
                using (StreamReader r = new StreamReader(GetPath + "/MagicNumber.txt"))
                {
                    string json = r.ReadToEnd();
                    MagicNumnberDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                }
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
            HttpContext.Current.Response.End();
        }

        return MagicNumnberDictionary;
    }


    public static List<ConfigSetting> GetConfigSettingList(long ID)
    {
        List<ConfigSetting> List = new List<ConfigSetting>();
        ConfigSetting eItem = new ConfigSetting();
        try
        {

            DataSet TempModuleDataSet = Common_SPU.fnGetConfigSetting(ID);
            foreach (DataRow item in TempModuleDataSet.Tables[0].Rows)
            {
                eItem = new ConfigSetting();
                eItem.ConfigID = Convert.ToInt32(item["ConfigID"]);
                eItem.Category = item["Category"].ToString();
                eItem.SubCategory = item["SubCategory"].ToString();
                eItem.Help = item["Help"].ToString();
                eItem.Remarks = item["Remarks"].ToString();
                eItem.ConfigKey = item["ConfigKey"].ToString();
                eItem.ConfigValue = item["ConfigValue"].ToString();
                List.Add(eItem);
            }
        }
        catch (Exception ex)
        {
            Common_SPU.LogError("Error during GetConfigSettingList. The query was executed :", ex.ToString(), "CP/GetConfigSettingList()", "", "", 0, "");
        }
        return List;
    }

    private static Dictionary<string, string> GetConfigSettingPair()
    {
        List<ConfigSetting> List = new List<ConfigSetting>();
        Dictionary<string, string> obj = new Dictionary<string, string>();
        List = GetConfigSettingList(0);
        foreach (var item in List)
        {
            obj.Add(item.ConfigKey, item.ConfigValue);
        }
        return obj;
    }
    public static string GetConfigValue(string KeyName)
    {
        string ValueType = "";
        if (GetConfigSettingPair().ContainsKey(KeyName))
        {
            ValueType = GetConfigSettingPair()[KeyName];
        }
        return ValueType;
    }

    public static string GetPageName()
    {
        string RR = clsApplicationSetting.GetWebConfigValue("ProjectName");
        var ReturnURL = HttpContext.Current.Request.Url.Segments;

        if (ReturnURL.Length > 1)
        {
            RR = RR + ": " + ReturnURL[ReturnURL.Length - 1];
        }
        return RR;
    }
    public static string GetIPAddress()
    {
        return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
    }
    public static string[] DecryptQueryString(string EncrptredValue)
    {
        long LoginID = 0;
        long.TryParse(GetSessionValue("LoginID"), out LoginID);
        string IPAddress = GetIPAddress();
        string[] MyMenu = null;
        string Value = "";
        try
        {
            if (!string.IsNullOrEmpty(EncrptredValue))
            {
                Value = clsCommon.Decrypt(EncrptredValue);
                if (Value.Contains("*"))
                {
                    MyMenu = Value.Split('*');
                }
            }
        }
        catch (Exception ex)
        {
            Common_SPU.LogError("Error during DecryptQueryString. The query was executed :", ex.ToString(), EncrptredValue, "ClsApplication", "ClsApplication", LoginID, IPAddress);
        }
        return MyMenu;
    }


    public static bool CreateAllJson()
    {
        bool val = false;
        try
        {
            CreateMenuJSon();
            CreateWeb_MenuJSon();
            val = true;
        }
        catch (Exception ex)
        {

        }
        return val;

    }


    //CMS start

    //public static bool CreateCMSJSon()
    //{
    //    long LoginID = 0;
    //    long.TryParse(GetSessionValue("LoginID"), out LoginID);
    //    bool myValue = false;
    //    string DirectoryName = clsApplicationSetting.GetPhysicalPath("json");
    //    string FileName = DirectoryName + "/CMS.txt";
    //    HomeModal Home = new HomeModal();
    //    GetWebResponse modal = new GetWebResponse();
    //    modal.IPAddress = GetIPAddress();
    //    modal.LoginID = LoginID;
    //    modal.Doctype = "";
    //    if (File.Exists(FileName))
    //    {
    //        File.Delete(FileName);
    //    }
    //    using (StreamWriter file = File.CreateText(FileName))
    //    {
    //        JsonSerializer serializer = new JsonSerializer();
    //        serializer.Serialize(file, Home.GetCMSContent(modal));
    //        myValue = true;
    //    }

    //    return myValue;
    //}

    //private static List<CMSContent> GetCMSContentJSON()
    //{
    //    string FileName = clsApplicationSetting.GetPhysicalPath("json") + "/CMS.txt";
    //    if (!File.Exists(FileName))
    //    {
    //        CreateCMSJSon();
    //    }
    //    List<CMSContent> items = new List<CMSContent>();
    //    if (File.Exists(FileName))
    //    {
    //        using (StreamReader r = new StreamReader(FileName))
    //        {
    //            string json = r.ReadToEnd();
    //            items = JsonConvert.DeserializeObject<List<CMSContent>>(json);
    //        }
    //    }
    //    return items;
    //}

    //public static List<CMSContent> GetCMSContent(string PageURL)
    //{
    //    long LoginID = 0;
    //    long.TryParse(GetSessionValue("LoginID"), out LoginID);
    //    List<CMSContent> Listobj = new List<CMSContent>();
    //    try
    //    {
    //        Listobj = GetCMSContentJSON().Where(x => x.PageURL.ToLower() == PageURL.ToLower()).ToList();
    //    }
    //    catch (Exception ex)
    //    {
    //        Common_SPU.LogError("Error during GetCMSContent. The query was executed :", ex.ToString(), "GetCMSContentJSON", "clsApplicationSetting", "clsApplicationSetting", LoginID, GetIPAddress());
    //    }
    //    return Listobj;
    //}

    public static List<DocumentType.List> GetDocTypeList()
    {
        long LoginID = 0;
        long.TryParse(GetSessionValue("LoginID"), out LoginID);
        List<DocumentType.List> result = new List<DocumentType.List>();
        MasterModal Master = new MasterModal();
        GetResponse Responsemodal = new GetResponse();
        Responsemodal.IPAddress = GetIPAddress();
        Responsemodal.LoginID = LoginID;
        result = Master.GetDocumentTypeList(Responsemodal).Where(x => x.IsActive).ToList();
        return result;
    }

    //CMS end


    //website Menu start

    public static bool CreateWeb_MenuJSon()
    {
        long LoginID = 0;
        long.TryParse(GetSessionValue("LoginID"), out LoginID);
        bool myValue = false;
        string DirectoryName = clsApplicationSetting.GetPhysicalPath("json");
        string FileName = DirectoryName + "/Menu.txt";
        HomeModal Home = new HomeModal();
        GetMenuResponse modal = new GetMenuResponse();
        modal.IPAddress = GetIPAddress();
        modal.LoginID = LoginID;
        if (File.Exists(FileName))
        {
            File.Delete(FileName);
        }
        using (StreamWriter file = File.CreateText(FileName))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, Home.GetMenu_Web(modal));
            myValue = true;
        }

        return myValue;
    }

    private static List<WebMenu.List> GetWeb_MenuJSON()
    {
        string FileName = clsApplicationSetting.GetPhysicalPath("json") + "/Menu.txt";
        if (!File.Exists(FileName))
        {
            CreateWeb_MenuJSon();
        }
        List<WebMenu.List> items = new List<WebMenu.List>();
        if (File.Exists(FileName))
        {
            using (StreamReader r = new StreamReader(FileName))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<WebMenu.List>>(json);
            }
        }
        return items;
    }

    public static List<WebMenu.List> GetWebMenu(string MenuType)
    {
        long LoginID = 0;
        long.TryParse(GetSessionValue("LoginID"), out LoginID);
        List<WebMenu.List> Listobj = new List<WebMenu.List>();
        try
        {
            if (!string.IsNullOrEmpty(MenuType))
            {
                Listobj = GetWeb_MenuJSON().Where(x => x.MenuType.ToLower() == MenuType.ToLower()).ToList();
            }
            else
            {
                Listobj = GetWeb_MenuJSON().ToList();
            }

        }
        catch (Exception ex)
        {
            Common_SPU.LogError("Error during GetWebMenu. The query was executed :", ex.ToString(), "GetWeb_MenuJSON", "clsApplicationSetting", "clsApplicationSetting", LoginID, GetIPAddress());
        }
        return Listobj;
    }
    // website Menu end


    public static bool CreateMenuJSon()
    {
        long LoginID = 0;
        long.TryParse(GetSessionValue("LoginID"), out LoginID);
        bool myValue = false;
        string DirectoryName = clsApplicationSetting.GetPhysicalPath("json");
        string FileName = DirectoryName + "/AdminMenu.txt";
        ToolsModal Tools = new ToolsModal();
        GetResponse modal = new GetResponse();
        modal.IPAddress = GetIPAddress();
        modal.LoginID = LoginID;
        if (File.Exists(FileName))
        {
            File.Delete(FileName);
        }
        using (StreamWriter file = File.CreateText(FileName))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, Tools.GetAdminMenuList(modal));
            myValue = true;
        }

        return myValue;
    }

    private static List<AdminMenu> GetMenuJSON()
    {
        string FileName = clsApplicationSetting.GetPhysicalPath("json") + "/AdminMenu.txt";
        if (!File.Exists(FileName))
        {
            CreateMenuJSon();
        }
        List<AdminMenu> items = new List<AdminMenu>();
        if (File.Exists(FileName))
        {
            using (StreamReader r = new StreamReader(FileName))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<AdminMenu>>(json);
            }
        }
        return items;
    }



    public static List<AdminModule> GetRoleWiseModuleList(string Type)
    {
        List<AdminModule> CP_LoginModuleList = new List<AdminModule>();
        AdminModule CP_LoginModuleItem;
        string SQL = "";
        long RoleID = 0, LoginID = 0;
        long.TryParse(clsApplicationSetting.GetSessionValue("RoleID"), out RoleID);
        long.TryParse(clsApplicationSetting.GetSessionValue("LoginID"), out LoginID);
        try
        {
            var jsonModal = GetMenuJSON().Where(x => x.RoleID == RoleID && x.R == true && x.ModuleID != 0).ToList();
            var FilterModule = jsonModal.GroupBy(x => x.ModuleID)
                     .Select(x => new
                     {
                         ModuleID = x.Key,
                         Type = x.Select(ex => ex.Type).FirstOrDefault(),
                         ModuleName = x.Select(ex => ex.ModuleName).FirstOrDefault(),
                         ModuleIcon = x.Select(ex => ex.ModuleIcon).LastOrDefault(),
                         ModulePriority = x.Select(ex => ex.ModulePriority).FirstOrDefault(),
                     })
                     .ToList().OrderBy(x => x.ModuleName).OrderBy(x => x.ModulePriority).ToList();

            if (!string.IsNullOrEmpty(Type))
            {
                FilterModule = jsonModal.GroupBy(x => x.ModuleID)
                     .Select(x => new
                     {
                         ModuleID = x.Key,
                         Type = x.Select(ex => ex.Type).FirstOrDefault(),
                         ModuleName = x.Select(ex => ex.ModuleName).FirstOrDefault(),
                         ModuleIcon = x.Select(ex => ex.ModuleIcon).LastOrDefault(),
                         ModulePriority = x.Select(ex => ex.ModulePriority).FirstOrDefault(),
                     })
                     .ToList().Where(x => x.Type == Type).OrderBy(x => x.ModuleName).OrderBy(x => x.ModulePriority).ToList();
            }

            foreach (var item in FilterModule)
            {
                CP_LoginModuleItem = new AdminModule();
                CP_LoginModuleItem.ModuleID = item.ModuleID;
                CP_LoginModuleItem.Type = item.Type;
                CP_LoginModuleItem.ModuleName = item.ModuleName;
                CP_LoginModuleItem.ModuleIcon = item.ModuleIcon;
                CP_LoginModuleItem.MainMenuList = GetLoginMenuList(item.ModuleID);
                CP_LoginModuleList.Add(CP_LoginModuleItem);
            }
        }
        catch (Exception ex)
        {
            Common_SPU.LogError("Error during GetRoleWiseModuleList. The query was executed :", ex.ToString(), "CP/GetRoleWiseModuleList()", "clsApplicationSetting", "clsApplicationSetting", LoginID, GetIPAddress());
        }
        return CP_LoginModuleList;
    }
    private static List<AdminMenu> GetLoginMenuList(long ModuleID, long ParentMenuID = 0)
    {
        string SQL = "";
        long RoleID = 0, LoginID = 0;
        long.TryParse(clsApplicationSetting.GetSessionValue("RoleID"), out RoleID);
        long.TryParse(clsApplicationSetting.GetSessionValue("LoginID"), out LoginID);

        List<AdminMenu> CP_LoginMenuList = new List<AdminMenu>();
        AdminMenu CP_LoginMenuItem;
        try
        {
            List<AdminMenu> jsonModal = GetMenuJSON().Where(x => x.RoleID == RoleID && x.R == true && x.ModuleID == ModuleID).ToList();
            jsonModal = jsonModal.Where(x => x.ParentMenuID == ParentMenuID).ToList();
            foreach (AdminMenu item in jsonModal.OrderBy(x => x.MenuName).OrderBy(x => x.MenuPriority).ToList())
            {
                CP_LoginMenuItem = new AdminMenu();
                CP_LoginMenuItem.MenuID = item.MenuID;
                CP_LoginMenuItem.ParentMenuID = item.ParentMenuID;
                CP_LoginMenuItem.ModuleID = item.ModuleID;
                CP_LoginMenuItem.MenuPriority = item.MenuPriority;
                CP_LoginMenuItem.MenuName = item.MenuName;
                CP_LoginMenuItem.MenuURL = item.MenuURL;
                CP_LoginMenuItem.Target = item.Target;
                CP_LoginMenuItem.IsChild = item.IsChild;
                if (item.IsChild == "Y")
                {
                    CP_LoginMenuItem.ChildMenuList = GetLoginMenuList(item.ModuleID, CP_LoginMenuItem.MenuID);
                }

                CP_LoginMenuList.Add(CP_LoginMenuItem);
            }
        }
        catch (Exception ex)
        {
            Common_SPU.LogError("Error during GetLoginMenuList. The query was executed :", ex.ToString(), "GetLoginMenuList", "clsApplicationSetting", "clsApplicationSetting", LoginID, GetIPAddress());
        }
        return CP_LoginMenuList;
    }

    public static PageViewPermission CheckPageViewPermission(string MenuID)
    {
        long RoleID = 0, MID = 0;
        PageViewPermission PageViewPermission = new PageViewPermission();
        try
        {
            long.TryParse(clsApplicationSetting.GetSessionValue("RoleID"), out RoleID);
            long.TryParse(MenuID, out MID);
            DataSet ds = new DataSet();
            var MenuItems = GetMenuJSON().Where(x => x.RoleID == RoleID && x.MenuID == MID).FirstOrDefault();
            if (MenuItems != null)
            {
                PageViewPermission.ReadFlag = MenuItems.R;
                PageViewPermission.WriteFlag = MenuItems.W;
                PageViewPermission.ModifyFlag = MenuItems.M;
                PageViewPermission.DeleteFlag = MenuItems.D;
                PageViewPermission.ExportFlag = MenuItems.E;
                SetSessionValue("Read", PageViewPermission.ReadFlag.ToString());
                SetSessionValue("Write", PageViewPermission.WriteFlag.ToString());
                SetSessionValue("Modify", PageViewPermission.ModifyFlag.ToString());
                SetSessionValue("Delete", PageViewPermission.DeleteFlag.ToString());
                SetSessionValue("Export", PageViewPermission.ExportFlag.ToString());
            }
        }
        catch (Exception ex)
        {
            Common_SPU.LogError("Error during CheckPageViewPermission. The query was executed :", ex.ToString(), "CheckPageViewPermission", "clsApplicationSetting", "clsApplicationSetting", 0, GetIPAddress());
        }
        return PageViewPermission;
    }


    public static List<MetaTags.Web> GetSEODetails()
    {
        List<MetaTags.Web> List = new List<MetaTags.Web>();
        MetaTags.Web Tagsobj = new MetaTags.Web();
        string PageURL = HttpContext.Current.Request.Url.AbsolutePath;
        try
        {
            IHomeHelper Home = new HomeModal();
            GetResponse getResponse = new GetResponse();
            getResponse.IPAddress = GetIPAddress();
            getResponse.Doctype = PageURL;
            var Result = Home.GetMetaTags_WebList(getResponse);
            if (Result.Count > 0)
            {
                foreach (var item in Result)
                {
                    Tagsobj = new MetaTags.Web();
                    Tagsobj.MetaName = item.MetaName;
                    Tagsobj.MetaValue = item.MetaValue;
                    List.Add(Tagsobj);
                }
            }
            if (!List.Any(x => x.MetaName.ToLower() == "title"))
            {
                Tagsobj = new MetaTags.Web();
                Tagsobj.MetaName = "Title";
                Tagsobj.MetaValue = GetPageName();
                List.Add(Tagsobj);
            }
            if (!List.Any(x => x.MetaName.ToLower() == "keyword"))
            {
                Tagsobj = new MetaTags.Web();
                Tagsobj.MetaName = "Keyword";
                Tagsobj.MetaValue = GetPageName();
                List.Add(Tagsobj);
            }
            if (!List.Any(x => x.MetaName.ToLower() == "description"))
            {
                Tagsobj = new MetaTags.Web();
                Tagsobj.MetaName = "Description";
                Tagsobj.MetaValue = GetPageName();
                List.Add(Tagsobj);
            }
        }
        catch (Exception ex)
        {
            Common_SPU.LogError("Error during GetSEODetails. The query was executed :", ex.ToString(), "ClsApplicationSetting", "ClsApplicationSetting", "ClsApplicationSetting", 0, GetIPAddress());
        }
        return List;

    }



    public static string GetBaseUrl()
    {
        var request = HttpContext.Current.Request;
        var appUrl = HttpRuntime.AppDomainAppVirtualPath;

        if (appUrl != "/")
            appUrl = "/" + appUrl;

        var baseUrl = string.Format("{0}://{1}{2}",
     request.Url.Scheme, //request.Url.Scheme gives https or http
    request.Url.Authority, //request.Url.Authority gives qawithexperts/com
     appUrl); //appUrl = /questions/111/ok-this-is-url

        return baseUrl; //this will return complete url
    }

}
