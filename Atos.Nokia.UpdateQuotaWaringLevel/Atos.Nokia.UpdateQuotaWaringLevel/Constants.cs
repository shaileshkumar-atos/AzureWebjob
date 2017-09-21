using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atos.Nokia.UpdateQuotaWaringLevel
{
    class Constants
    {
       public static string _strTenentAdminUrl= System.Configuration.ConfigurationManager.AppSettings["TanentAdminSiteURL"];
       public static string _strQuotaWarningPercentage= System.Configuration.ConfigurationManager.AppSettings["QuotaWarningPercentage"];
       public static string _strQuotaWarningExistingPercentage = System.Configuration.ConfigurationManager.AppSettings["QuotaWarningExistingPercentage"];
       public static string _strQuota = System.Configuration.ConfigurationManager.AppSettings["Quota"];
       public static string _strLogPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
       public static string _strFileName = System.Configuration.ConfigurationManager.AppSettings["FileName"];
       public static string _strLogURL = System.Configuration.ConfigurationManager.AppSettings["LogURL"];
       public static string _strLogLibraryName = System.Configuration.ConfigurationManager.AppSettings["LogLibraryName"];
       public static string _strBatchUpdate= System.Configuration.ConfigurationManager.AppSettings["batchupdate"];
       public static string _strUrlToUpdateQuotaWarning= System.Configuration.ConfigurationManager.AppSettings["urltoupdatequotawarning"];
       public static string _strUpdateStartsFrom = System.Configuration.ConfigurationManager.AppSettings["UpdateStartsFrom"];

        public static string _strServerResourceQuota = System.Configuration.ConfigurationManager.AppSettings["ServerResourceQuota"];
        public static string _strUpdatestrServerResourceQuota = System.Configuration.ConfigurationManager.AppSettings["UpdateServerResourceQuota"];
    }
}
