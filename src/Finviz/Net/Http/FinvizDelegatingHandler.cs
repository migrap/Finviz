using System.Net;
using System.Net.Http;

namespace Finviz.Net.Http {
    internal class FinvizDelegatingHandler : DelegatingHandler {
        public FinvizDelegatingHandler() {
            InnerHandler = new HttpClientHandler {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.None,
            };
        }
    }
}