
using System;
using System.Collections.Generic;

namespace ivNet.WebStore.Helpers
{
    public static class PayPalLog
    {
        private static readonly string DebugFilename = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "logs\\" +
                                                       "paypal-debug-" + DateTime.Now.ToString("yyyy.MM.dd") + ".log";

        private static readonly string ErrorFilename = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "logs\\" +
                                                       "paypal-error-" + DateTime.Now.ToString("yyyy.MM.dd") + ".log";
      
        public static void Debug(string message)
        {
            var sw = new System.IO.StreamWriter(DebugFilename, true);
            sw.WriteLine(string.Format("{0} {1}", DateTime.Now, message));
            sw.Close();           
        }

        public static void Error(Exception ex)
        {
            var sw = new System.IO.StreamWriter(ErrorFilename, true);
            sw.WriteLine(string.Format("{0} {1} [{2}]", DateTime.Now, ex.Message, ex.InnerException));
            sw.Close();
        }

        public static List<string> GetAll()
        {
            var logs = new List<string>();
            logs.AddRange(GetErrors());
            logs.AddRange(GetDebug());

            return logs;
        }

        public static List<string> GetErrors()
        {
            var logs = new List<string>();

            var sr = new System.IO.StreamReader(ErrorFilename, true);
            while (!sr.EndOfStream)
            {
                logs.Add(string.Format("Error: {0}", sr.ReadLine()));
            }
            sr.Close();

            return logs;
        }

        public static List<string> GetDebug()
        {
            var logs = new List<string>();

            var sr = new System.IO.StreamReader(DebugFilename, true);
            while (!sr.EndOfStream)
            {
                logs.Add(string.Format("Debug: {0}", sr.ReadLine()));
            }
            sr.Close();

            return logs;
        }
    }
}