using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace EventSourcingSampleWithCQRSandMediatr.Tests.Helpers
{
    public static class HttpClientExtensions
    {
        public static HttpContent ToContent(this object payload)
        {
            return new StringContent(JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");
        }
    }
}
