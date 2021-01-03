using System;
using System.IO;

namespace PaySys.Exceptions
{
    public class ExceptionLogging
    {
        private static String ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;

        public static void SendErrorToText(Exception ex)
        {
            var line = Environment.NewLine + Environment.NewLine;

            ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            Errormsg = ex.GetType().Name.ToString();
            extype = ex.GetType().ToString();

            ErrorLocation = ex.Message.ToString();

            try
            {

              
                string realPath = @"C:\FINTRACK";
                string appLog = "FinTrackExceptions";

                var logPath = realPath + Convert.ToString(appLog) + DateTime.Today.ToString("dd -MM-yy") + ".txt";
                if (!File.Exists(logPath))
                {
                    File.Create(logPath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation + line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line;
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                e.ToString();

            }

        }
    }
}