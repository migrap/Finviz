using Finviz.Net.Http.Configurators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Finviz {
    public static partial class Extensions {
        internal static string FormatWith(this string input, params object[] formatting) {
            return string.Format(input, formatting);
        }

        internal static void Foreach<T>(this IEnumerable<T> self, Action<T> action) {
            var items = self.ToArray();
            for (int i = 0; i < items.Length; i++) {
                var item = items[i];
                action(item);
            }
        }

        internal static string ToQueryString(this Parameters self) {
            if (self == null) {
                return string.Empty;
            }

            return self.Select(x => "{0}={1}".FormatWith(x.Key.UrlEncode(), x.Value.UrlEncode())).Join("&");
        }

        /// <summary>
        /// Uses Uri.EscapeDataString() based on recommendations on MSDN
        /// http://blogs.msdn.com/b/yangxind/archive/2006/11/09/don-t-use-net-system-uri-unescapedatastring-in-url-decoding.aspx
        /// </summary>
        internal static string UrlEncode(this string self) {
            return Uri.EscapeDataString(self);
        }

        internal static string UrlEncode(this object self) {
            return UrlEncode(self.ToString());
        }

        internal static string Join(this IEnumerable<object> source, string seperator) {
            return string.Join(seperator, source);
        }

        internal static Uri Append(this Uri self, params object[] segments) {
            return new Uri(segments.Aggregate(self.AbsoluteUri, (current, segment) => string.Format("{0}/{1}", current.TrimEnd('/'), segment)));
        }

        internal static bool IsNotDefault<T>(this T self) {
            return !default(T).Equals(self);
        }

        private static TResult Configure<TSource, TResult>(Action<TSource> configure) where TResult : TSource, new() {
            var result = new TResult();
            configure(result);
            return result;
        }

        internal static Task<HttpResponseMessage> SendAsync(this HttpClient client, Action<IHttpRequestMessageConfigurator> configure) {
            var request = Configure<IHttpRequestMessageConfigurator, HttpRequestMessageConfigurator>(configure).Build();
            return client.SendAsync(request);
        }

        internal static IHttpRequestMessageConfigurator Address(this IHttpRequestMessageConfigurator self, string format, params object[] args) {
            return self.Address(format.FormatWith(args));
        }

        internal static IHttpRequestMessageConfigurator Content(this IHttpRequestMessageConfigurator self, object value, MediaTypeFormatter formatter) {
            return self.Content(new ObjectContent(value.GetType(), value, formatter));
        }

        internal static Task<T> ReadAsAsync<T>(this Task<HttpResponseMessage> message, params MediaTypeFormatter[] formatters) {
            var response = message
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsAsync<T>(formatters);
        }

        internal static Task<string> ReadAsStringAsync(this Task<HttpResponseMessage> message) {
            var response = message
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync();
        }

        internal static Task<T> ReadAsAsync<T>(this HttpResponseMessage message, MediaTypeFormatter formatter) {
            return ReadAsAsync<T>(message.Content, formatter);
        }

        internal static Task<T> ReadAsAsync<T>(this HttpContent content, MediaTypeFormatter formatter) {
            return content.ReadAsAsync<T>(new[] { formatter });
        }
    }
}
