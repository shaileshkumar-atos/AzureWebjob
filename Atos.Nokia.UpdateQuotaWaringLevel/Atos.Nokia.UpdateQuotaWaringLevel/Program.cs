using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atos.Nokia.UpdateQuotaWaringLevel
{
    class Program
    {
        public static string _strLogPath = Constants._strLogPath;
        public static string _strFileName = Constants._strFileName;
        static void Main(string[] args)
        {
            if (Constants._strBatchUpdate.ToLower() == "true")
            {
                Common.UpdateQuotaWarningLevel();
            }
            else
            {
                Common.UpdateQuotaWarningLevelBasedOnURL(Constants._strUrlToUpdateQuotaWarning);
            }
            DateTime dtJobStartTime = System.DateTime.Now;
            string sLog_FileName = "SiteCollectionStatus" + dtJobStartTime.Hour.ToString() + "_" + dtJobStartTime.Minute.ToString() + "-" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + ".log";
            string date = Convert.ToString(System.DateTime.Now.ToShortDateString());
            //string LogFile = _strLogPath + "\\" + date + _strFileName;
            //_strFileName = date + _strFileName; 
            Common.UploadFiles(sLog_FileName); 
        }
    }
}
