using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using LightningTalk.PostModels;
using LightningTalk.Utils;

namespace LightningTalk
{
    public static class Sessions
    {
        [FunctionName("Sessions")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var data = await RequestDecoder.Decode<SessionCreatePostModel>(req);

            var jwt = Jwt.Decode(data.password);
            if (jwt.subject != data.username)
            {
                throw new HttpResponseException(req.CreateErrorResponse(HttpStatusCode.Forbidden, "Password not valid for that username"));
            }

            var response = new
                {
                    message = "Hello " + data.username
                };
            return req.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
