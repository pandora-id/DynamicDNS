using System.Net.Http;

namespace DynamicDNS.Common
{
    public class ExtendedHttpMethod : HttpMethod
    {
        public ExtendedHttpMethod(string method) : base(method) { }

        public static HttpMethod Patch { get; } = new HttpMethod("PATCH");
    }
}
