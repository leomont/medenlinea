using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Medinlinea.Models
{
    public class SystemLog
    {
        private string sLogFormat;
        private string sErrorTime;

        public SystemLog()
        {
            //sLogFormat se usa para crear el formato del archivo :
            // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
            sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            //variables para el formate el nombre "
            //filename : ErrorLogYYYYMMDD
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            sErrorTime = sYear + sMonth + sDay;
        }

        public void ErrorLog(string sErrMsg)
        {
            try
            {
                string sPathName = System.Web.HttpContext.Current.Server.MapPath("~");
                sPathName += "Logs\\ErrorLog";
                StreamWriter sw = new StreamWriter(sPathName + sErrorTime, true);
                sw.WriteLine(sLogFormat + sErrMsg);
                sw.Flush();
                sw.Close();
            }
            catch (Exception) { }
        }
    }
}