using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using TaxEncryptDecrypt;
using Newtonsoft.Json;
using DataAccess.Setting;

public class clsCommon
{
    public static string Encrypt(string clearText)
    {
        try
        {
            if (!string.IsNullOrEmpty(clearText))
            {
                string EncryptionKey = "RAISOFT";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }
        catch (Exception ex)
        {
           // Common_SPU.LogError("Error during Encrypt. The query was executed :", ex.ToString(), "ClsCommon()", "ClsCommon", "ClsCommon", 0, GetIPAddress());
        }
        return clearText;
    }
    public static string Decrypt(string cipherText)
    {
        try
        {
            if (!string.IsNullOrEmpty(cipherText))
            {
                string EncryptionKey = "RAISOFT";
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
        }
        catch (Exception ex)
        {
           // Common_SPU.LogError("Error during Decrypt. The query was executed :", ex.ToString(), "ClsCommon()", "ClsCommon", "ClsCommon", 0, GetIPAddress());
        }
        return cipherText;
    }

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
           
        }
        return functionReturnValue;

    }

    public static string EnsureString(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            str = "";
        }
        return str.Replace("'", "''").Replace("\"", "" + "").Trim();
    }

    public static int UpdateIsActiveCommon(string ColomnName, string TableName, string WhereColomn, string ID, string Value,long LoginID,string IPAddress)
    {
        string SQL = "";
        int status = -1;
        try
        {
            SQL = "Update " + TableName + "  Set " + ColomnName + "='" + Value + "', modifiedat=GetDate(),modifiedby=" + LoginID + ", IPAddress='" + IPAddress + "' Where " + WhereColomn + "=" + ID;
            clsDataBaseHelper.ExecuteNonQuery(SQL);
            status = Convert.ToInt32(ID);
        }
        catch (Exception ex)
        {
            Common_SPU.LogError("Error during UpdateIsActiveCommon. The query was executed :" + SQL, ex.ToString(), "ClsCommon/UpdateIsActiveCommon()", "ClsCommon", "ClsCommon", LoginID,IPAddress);
        }

        return status;
    }
    public static bool UpdateColom_Common(string ColomnName, string TableName, string WhereColom, string ID, string Value, long LoginID, string IPAddress)
    {
        bool ret = false;
        int SaveID = 0;
        string SQL = "";
        try
        {
            SQL = "update " + TableName + " set " + ColomnName + "='" + Value + "', modifiedat=getdate(),modifiedby=" + LoginID + ", IPAddress='" + IPAddress + "' where " + WhereColom + "=" + ID;
            if (clsDataBaseHelper.ExecuteNonQuery(SQL) > 0)
            {
                ret = true;
            }

        }
        catch (Exception ex)
        {
            Common_SPU.LogError("Error during UpdateColom_Common. The query was executed :", ex.ToString(), SQL, "clsCommon", "common class", LoginID, IPAddress);
        }
        return ret;

    }


    public static string RemoveSpecialCharters(string URL)
    {
        if (!string.IsNullOrEmpty(URL))
        {
            string SpecialCharter = ".,~,@,$,%,^,&,*,(,),=,+,*,<,>,'";
            string[] StrArr = SpecialCharter.Split(',');
            foreach (var item in StrArr)
            {
                URL = URL.Replace(item, "");
            }
            URL = URL.Replace(" ", "_").ToLower();
        }

        return URL;
    }


}
