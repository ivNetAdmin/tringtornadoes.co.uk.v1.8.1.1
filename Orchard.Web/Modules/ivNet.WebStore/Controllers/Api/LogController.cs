
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ivNet.WebStore.Helpers;

namespace ivNet.WebStore.Controllers.Api
{
    public class LogController : ApiController
    {
        public HttpResponseMessage Get()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                    PayPalLog.GetAll());
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                       ex.Message);
            }
        }

        public HttpResponseMessage Get(string id)
        {
            try
            {                
                switch (id.ToLowerInvariant())
                {
                    case "errors":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            PayPalLog.GetErrors());
                    case "debug":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            PayPalLog.GetDebug());
                }

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                       ex.Message);
            }
        }
    }
}