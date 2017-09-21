using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atos.Nokia.UpdateQuotaWaringLevel
{
    class Logger
    {
        public static string _strLogPath = Constants._strLogPath;
        public static string _strFileName = Constants._strFileName;
        /// <summary>
        /// This function will create a log file in the folder where badge csv file copied from the shared folder and track the badge details and exceptions
        /// </summary>
        public static void WriteLog(string message)
        {
            string date = System.DateTime.Now.ToShortDateString();
            try
            {
                string LogFile = _strLogPath + "/" + date + _strFileName;
                if (!System.IO.File.Exists(LogFile))
                {
                    FileStream fs = System.IO.File.Create(LogFile);
                    fs.Close();
                }
                if (File.Exists(LogFile))
                {
                    using (StreamWriter sw = new StreamWriter(LogFile, true))
                    {
                        sw.Write("\r\nLog Entry : ");
                        sw.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                            DateTime.Now.ToLongDateString());
                        sw.WriteLine("Message  :{0}", message);
                        sw.WriteLine("-------------------------------");
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex == null)
                {
                    throw;
                }
                else
                {                    
                    //WriteLog(Convert.ToString(ex));
                }
            }
        }
    }
}
