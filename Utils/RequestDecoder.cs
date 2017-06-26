using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LightningTalk.Utils
{
    internal static class RequestDecoder
    {
        internal static Task<T> Decode<T>(HttpRequestMessage req)
            where T : new()
        {
            if (req.RequestUri.Scheme != "https")
            {
                throw new HttpResponseException(req.CreateErrorResponse(HttpStatusCode.Forbidden, "HTTPS required"));
            }

            return Get<T>(req);
        }

        private static async Task<T> Get<T>(HttpRequestMessage req)
            where T : new()
        {
            switch (req.Content.Headers.ContentType.MediaType)
            {
                case "application/json":
                    return await req.Content.ReadAsAsync<T>();

                case "multipart/form-data":
                    var formData = (await req.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider())).FormData;
                    var ret = new T();
                    foreach (string key in formData)
                    {
                        ret.GetType().GetProperty(key).SetValue(ret, formData[key]);
                    }
                    return ret;

                default:
                    throw new HttpResponseException(req.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "JSON or FormData please"));
            }
        }
    }
}
