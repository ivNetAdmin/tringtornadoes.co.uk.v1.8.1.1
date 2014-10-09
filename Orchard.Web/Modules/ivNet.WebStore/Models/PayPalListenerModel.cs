
using System.IO;
using System.Net;
using System.Text;
using ivNet.WebStore.Helpers;
using Newtonsoft.Json;

namespace ivNet.WebStore.Models
{
    public class PayPalListenerModel
    {
        public PayPalPaymentInfo PayPalPaymentInfo { get; set; }

        public void GetStatus(byte[] parameters)
        {

            //verify the transaction             
            var status = Verify(true, parameters);

            if (status == "VERIFIED")
            {

                //check that the payment_status is Completed                 
                if (PayPalPaymentInfo.payment_status.ToLower() == "completed")
                {

                    //check that txn_id has not been previously processed to prevent duplicates                      

                    //check that receiver_email is your Primary PayPal email                                          

                    //check that payment_amount/payment_currency are correct                       

                    //process payment/refund/etc                     

                }
                else if (status == "INVALID")
                {                    
                    //log for manual investigation             
                }
                else
                {
                    PayPalLog.Debug(string.Format("Unknown status[{0}] {1}",status,JsonConvert.SerializeObject(PayPalPaymentInfo)));
                }

            }

        }

        private string Verify(bool isSandbox, byte[] parameters)
        {

            var response = "";
            try
            {

                var url = isSandbox
                    ? "https://www.sandbox.paypal.com/cgi-bin/webscr"
                    : "https://www.paypal.com/cgi-bin/webscr";

                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";

                //must keep the original intact and pass back to PayPal with a _notify-validate command
                string data = Encoding.ASCII.GetString(parameters);
                data += "&cmd=_notify-validate";

                webRequest.ContentLength = data.Length;

                //Send the request to PayPal and get the response                 
                using (
                    var streamOut = new StreamWriter(webRequest.GetRequestStream(), System.Text.Encoding.ASCII)
                    )
                {
                    streamOut.Write(data);
                    streamOut.Close();
                }

                using (var streamIn = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    response = streamIn.ReadToEnd();
                    streamIn.Close();
                }

            }
            catch
            {
            }

            return response;
        }
    }
}