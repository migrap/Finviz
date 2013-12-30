using System;
using System.Net.Http;

namespace Finviz.Net.Http.Configurators {
    internal class HttpRequestMessageConfigurator : IHttpRequestMessageConfigurator {
        private HttpRequestMessage _request = new HttpRequestMessage();

        public HttpRequestMessageConfigurator() {            
        }

        public IHttpRequestMessageConfigurator Method(HttpMethod value) {
            _request.Method = value;
            return this;
        }

        public IHttpRequestMessageConfigurator Content(HttpContent value) {
            _request.Content = value;
            return this;
        }

        public IHttpRequestMessageConfigurator Properties(string name, object value) {
            _request.Properties.Add(name, value);
            return this;
        }

        public IHttpRequestMessageConfigurator Address(string value) {
            _request.RequestUri = new Uri(value);
            return this;
        }

        public IHttpRequestMessageConfigurator Header(string name, string value) {
            _request.Headers.Add(name, value);
            return this;
        }

        public HttpRequestMessage Build() {
            return _request;
        }
    }
}
