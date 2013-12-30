using Finviz.Net.Http;
using Finviz.Net.Http.Formatters;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Securities = System.Collections.Generic.List<Finviz.Models.Security>;

namespace Finviz {
    public partial class FinvizClient : IDisposable {
        private readonly MediaTypeFormatter _formatter = new FinvizMediaTypeFormatter();        
        private HttpMessageHandler _handler;
        private HttpClient _client;
        private volatile bool _disposed;

        public FinvizClient(string uri = "http://finviz.com/export.ashx") {
            _handler = new FinvizDelegatingHandler();
            _client = new HttpClient(_handler);
            _client.BaseAddress = new Uri(uri);
        }

        ~FinvizClient() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if(_disposed) {
                return;
            }

            if(disposing) {
            }
            _disposed = true;
        }

        public Task<Securities> GetSecuritiesAsync() {
            return _client.SendAsync(x => { })
                .ReadAsAsync<Securities>(_formatter);
        }
    }
}