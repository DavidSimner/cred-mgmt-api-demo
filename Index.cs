using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace LightningTalk
{
    public static class Index
    {
        [FunctionName("Index")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                log.Info("C# HTTP trigger function processed a request.");

                var file = new FileStream(@"D:\home\site\wwwroot\bin\index.html", FileMode.Open, FileAccess.Read, FileShare.Read);
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Content = new StreamContent(file);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return response;
            }
            catch (HttpResponseException error)
            {
                return error.Response;
            }
        }
    }
}
