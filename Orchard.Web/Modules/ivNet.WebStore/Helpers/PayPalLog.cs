
using System;

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
    }
}