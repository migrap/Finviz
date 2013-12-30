using System;
using System.Collections.Generic;
using System.IO;

namespace Finviz.IO {
    internal class DelimitedStringReader : StringReader {
        private string[] _delimiters;
        private readonly Func<string, string[]> _splitter;
        private IDictionary<string, int> _columns;
        private string[] _parts;

        public DelimitedStringReader(string s, params string[] delimiters)
            : this(s, false, delimiters) {
        }


        public DelimitedStringReader(string s, bool firstRowHasColumns, params string[] delimiters)
            : base(s) {
            _delimiters = delimiters;
            _splitter = new Func<string, string[]>((l) => {
                return l.Split(_delimiters, StringSplitOptions.None);
            });

            if(firstRowHasColumns) {
                Initialize();
            }
        }

        public DelimitedStringReader(string s, bool firstRowHasColumns, Func<string, string[]> splitter)
            : base(s) {
            _delimiters = null;
            _splitter = splitter;

            if(firstRowHasColumns) {
                Initialize();
            }
        }

        private void Initialize() {
            var line = this.ReadLine();
            _columns = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            if(null != line) {
                for(int i = 0; i < _parts.Length; i++) {
                    _columns.Add(_parts[i], i);
                }
            }
        }

        public override string ReadLine() {
            var line = base.ReadLine();
            _parts = (null == line) ? null : _splitter(line);
            return line;
        }

        public string[] Parts {
            get { return _parts; }
        }

        public IDictionary<string, int> Columns {
            get { return _columns; }
        }
    }
}
