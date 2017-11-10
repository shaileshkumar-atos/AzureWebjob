using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atos.Nokia.UpdateQuotaWaringLevel
{
    class Common
    {
        public static Uri TenentAdminSiteUrl = null;
        public static Int32 QuotaWarningPercentage = 0;
        public static Int32 QuotaWarningExistingPercentage = 0;
        public static double Quota = 0;
        public static string _strCurrentWarningLevel;
        public static string _strLogPath = Constants._strLogPath;
        public static string _strFileName = Constants._strFileName;
        public static string _strLogURL = Constants._strLogURL;
        public static string _strLogLibraryName = Constants._strLogLibraryName;
        public static Int32 _intUpdateCount = 1;
        public static Int32 _intItemCount = 0;
        public static Int32 ServerResourceQuota = 0;
        public static Int32 UpdatedServerResourceQuota = 0;
        public static StringBuilder sbFullLog = new StringBuilder();

        public static void UpdateQuotaWarningLevel()
        {
            try
            {
                TenentAdminSiteUrl = new Uri(Constants._strTenentAdminUrl);
                string realm = TokenHelper.GetRealmFromTargetUrl(TenentAdminSiteUrl);
                string accessToken = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, TenentAdminSiteUrl.Authority, realm).AccessToken;
                SPOSitePropertiesEnumerable prop = null;
                using (ClientContext ctx = TokenHelper.GetClientContextWithAccessToken(TenentAdminSiteUrl.ToString(), accessToken))
                {
                    Tenant tenant = new Tenant(ctx);
                    int startIndex = 0;
                    while (prop == null || prop.Count > 0)
                    {
                        prop = tenant.GetSiteProperties(startIndex, true);
                        ctx.Load(prop);
                        ctx.ExecuteQuery();
                        int count = 0;
                        foreach (SiteProperties sp in prop)
                        {
                            _intItemCount++;
                            count++;
                            Update(sp.Url, ctx, tenant, true, count, _intItemCount);

                        }
                        startIndex += prop.Count;
                    }
                    // Logger.WriteLog("================================== END ====================================");
                    string LogFile = _strLogPath + "\\" + _strFileName;
                    //UploadFiles(LogFile, _strFileName);
                }
            }
            catch (Exception ex)
            {
                //Logger.WriteLog(ex.ToString());
            }
        }
        public static void UpdateQuotaWarningLevelBasedOnURL(string url)
        {
            try
            {
                TenentAdminSiteUrl = new Uri(Constants._strTenentAdminUrl);
                string realm = TokenHelper.GetRealmFromTargetUrl(TenentAdminSiteUrl);
                string accessToken = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, TenentAdminSiteUrl.Authority, realm).AccessToken;
                SPOSitePropertiesEnumerable prop = null;
                using (ClientContext ctx = TokenHelper.GetClientContextWithAccessToken(TenentAdminSiteUrl.ToString(), accessToken))
                {
                    Tenant tenant = new Tenant(ctx);
                    string _strsites = url;
                    int count = 0;
                    if (_strsites.Contains(";"))
                    {
                        string[] sites = _strsites.Split(';');
                        foreach (string site in sites)
                        {

                            count++;
                            Update(site, ctx, tenant, true, count, _intItemCount);
                        }
                    }
                    else
                    {
                        Update(_strsites, ctx, tenant, true, 1, _intItemCount);
                    }
                    string LogFile = _strLogPath + "\\" + _strFileName;
                    
                    //UploadFiles(LogFile, _strFileName); 
                }
            }
            catch (Exception ex)
            {
                //Logger.WriteLog(ex.ToString());
            }
        }
        //public static void Update(string url, ClientContext ctx, Tenant tenant,bool isvalid, Int32 count,Int32 ItemCount)
        //{
        //    try
        //    {
        //        if (ItemCount > Convert.ToInt32(Constants._strUpdateStartsFrom))
        //        {
        //            QuotaWarningPercentage = Convert.ToInt32(Constants._strQuotaWarningPercentage);
        //            QuotaWarningExistingPercentage = Convert.ToInt32(Constants._strQuotaWarningExistingPercentage);
        //            Quota = Convert.ToDouble(Constants._strQuota);
        //            var siteProperties = tenant.GetSitePropertiesByUrl(url, true);
        //            ctx.Load(siteProperties);
        //            ctx.ExecuteQueryRetry();
        //            if (siteProperties.StorageMaximumLevel == Quota && (siteProperties.StorageWarningLevel * 100) / (siteProperties.StorageMaximumLevel) == QuotaWarningExistingPercentage)
        //            {
        //                _strCurrentWarningLevel = siteProperties.StorageWarningLevel.ToString();
        //                siteProperties.StorageWarningLevel = (QuotaWarningPercentage * siteProperties.StorageMaximumLevel) / 100;
        //                siteProperties.Update();
        //                tenant.Context.ExecuteQueryRetry();
        //                //Logger.WriteLog(count + "> Site  [" + url + "]  Current Warning Level= " + _strCurrentWarningLevel + "  Updated Warning Level= " + siteProperties.StorageWarningLevel + " Current Warning %= " + siteProperties.StorageWarningLevel * 100 / siteProperties.StorageMaximumLevel + "  [Lock State= " + siteProperties.LockState + "]     " + _intUpdateCount);
        //                _intUpdateCount++;
        //            }
        //            else
        //            {
        //                //Logger.WriteLog(count + "> The Site  [" + url + "]  With Quota Maximum Level= " + siteProperties.StorageMaximumLevel + "  Quota Warning Level= " + siteProperties.StorageWarningLevel + "  Not Updated" + " Current Warning %= " + siteProperties.StorageWarningLevel * 100 / siteProperties.StorageMaximumLevel + "  [Lock State= " + siteProperties.LockState + "]");
        //            }
        //        }
        //    }

        //    catch(Exception ex)
        //    {
        //       //Logger.WriteLog(ex.ToString());
        //    }
        //}
        public static void Update(string url, ClientContext ctx, Tenant tenant, bool isvalid, Int32 count, Int32 ItemCount)
        {
            try
            {
                if (ItemCount > Convert.ToInt32(Constants._strUpdateStartsFrom))
                {
                    ServerResourceQuota = Convert.ToInt32(Constants._strServerResourceQuota);
                    ServerResourceQuota = Convert.ToInt32(Constants._strUpdatestrServerResourceQuota);
                    var siteProperties = tenant.GetSitePropertiesByUrl(url, true);
                    ctx.Load(siteProperties);
                    ctx.ExecuteQueryRetry();
                    if (siteProperties.Status == "Active")
                    {
                        AppendTextToLog("Site:" + url + " Is Active");
                        //Logger.WriteLog("Site:" + url + " Is Active");
                        _intUpdateCount++;
                    }
                    else
                    {
                        AppendTextToLog("Site:" + url + " Is not Active");
                        //Logger.WriteLog(count + "> The Site  [" + url + "]  With Quota Maximum Level= " + siteProperties.StorageMaximumLevel + "  Quota Warning Level= " + siteProperties.StorageWarningLevel + "  Not Updated" + " Current Warning %= " + siteProperties.StorageWarningLevel * 100 / siteProperties.StorageMaximumLevel + "  [Lock State= " + siteProperties.LockState + "]");
                    }
                }
            }

            catch (Exception ex)
            {
                AppendTextToLog(ex.ToString());
                //Logger.WriteLog(ex.ToString());
            }
        }
        public static void AppendTextToLog(string sLogText)
        {
           
                    sbFullLog.Append(sLogText);
                    sbFullLog.AppendLine();
                   
        }
        public static void UploadFiles(string filename)
        {
            
                Console.WriteLine("1");
            byte[] ByteArrayFullLog = System.Text.Encoding.UTF8.GetBytes(sbFullLog.ToString());
            Console.WriteLine("2");
            // Failure Log Stream Conversion
            using (MemoryStream StrmFullLog = new MemoryStream(ByteArrayFullLog))
            {
                Console.WriteLine("3");
                //string s1path = oLogFolder.ServerRelativeUrl + "/" + sFileName;

                // Create new File in SP and bind contents from the memory stream
                FileCreationInformation flciNewLogFile = new FileCreationInformation();
                Console.WriteLine("4");
                flciNewLogFile.ContentStream = StrmFullLog;
                Console.WriteLine("5");
                flciNewLogFile.Url = filename;
                Console.WriteLine("6");
                flciNewLogFile.Overwrite = true;
                Console.WriteLine("7");
                try
                {
                    //string FILE = file;
                    string webSPOUrl = _strLogURL;
                    Uri SiteUrl = new Uri(webSPOUrl);
                    Console.WriteLine("8");
                    string realm = TokenHelper.GetRealmFromTargetUrl(SiteUrl);
                    string accessToken = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, SiteUrl.Authority, realm).AccessToken;
                    Console.WriteLine("9");
                    using (ClientContext ctx = TokenHelper.GetClientContextWithAccessToken(SiteUrl.ToString(), accessToken))
                    {
                        Web web = ctx.Web;
                        //FileCreationInformation newFile = new FileCreationInformation();
                        //newFile.Content = System.IO.File.ReadAllBytes(FILE);
                        //newFile.Url = filename;
                        //newFile.Overwrite = true;
                        List docs = web.Lists.GetByTitle(_strLogLibraryName);
                        Console.WriteLine("10");
                        Microsoft.SharePoint.Client.File uploadFile = docs.RootFolder.Files.Add(flciNewLogFile);
                        Console.WriteLine("11");
                        ctx.ExecuteQuery();
                        Console.WriteLine("12");
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }

            }




        }
    }
}
