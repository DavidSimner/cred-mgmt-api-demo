using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using LightningTalk.PostModels;
using LightningTalk.ResponseModels;
using LightningTalk.Utils;

namespace LightningTalk
{
    public static class Tokens
    {
        [FunctionName("Tokens")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                log.Info("C# HTTP trigger function processed a request.");

                var data = await RequestDecoder.Decode<TokenCreatePostModel>(req);

                var response = new TokenResponseModel
                    {
                        id = data.email,
                        name = GetName(data.email),
                        iconURL = Gravatar.GetImageUrl(data.email),
                        password = new Jwt(data.email).Encode()
                    };
                return req.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (HttpResponseException error)
            {
                return error.Response;
            }
        }

        private static string GetName(string email)
        {
            var parts = email.Substring(0, email.IndexOf('@')).Split('.')
                .Select(x => x.Substring(0, 1).ToUpperInvariant() + x.Substring(1).ToLowerInvariant())
                .ToList();
            return string.Join(" ", parts);
        }
    }
}
