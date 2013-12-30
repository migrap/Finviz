using Finviz.IO;
using Finviz.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Finviz.Net.Http.Formatters {
    internal class FinvizMediaTypeFormatter : BufferedMediaTypeFormatter {
        public FinvizMediaTypeFormatter() {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));
        }

        public override bool CanReadType(Type type) {
            return true;
        }

        public override bool CanWriteType(Type type) {
            return true;
        }

        public override object ReadFromStream(Type type, Stream stream, HttpContent content, IFormatterLogger logger) {
            if(typeof(IEnumerable<Models.Security>).IsAssignableFrom(type)) {
                return ReadSecuritiesFromStream(type, stream, content, logger);
            }
            throw new NotImplementedException();
        }

        private object ReadSecuritiesFromStream(Type type, Stream stream, HttpContent content, IFormatterLogger logger) {
            var securities = new List<Security>();
            var line = (string)null;
            using(var reader = new DelimitedStringReader(new StreamReader(stream).ReadToEnd(), true, "\",\"", ",")) {
                var columns = reader.Columns;

                while(null != (line = reader.ReadLine())) {
                    var parts = reader.Parts.Select(x => x.Replace("\"", String.Empty)).ToArray();

                    var symbol = parts[columns["Ticker"]];
                    var company = parts[columns["Company"]];
                    var sector = parts[columns["Sector"]];
                    var industry = parts[columns["Industry"]];
                    var country = parts[columns["Country"]];

                    var security = new Security {
                        Symbol = symbol,
                        Compnay = company,
                        Sector = sector,
                        Industry = industry,
                        Country = country,
                    };

                    securities.Add(security);
                }
            }
            return securities;
        }
    }
}