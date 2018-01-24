using System;
using System.Collections.Generic;
using System.Text;

namespace GeminiLab.Core2.ML.Json {
    internal class JsonTokenQueue {
        private readonly IEnumerator<JsonToken> _tokens;
        private JsonToken _cache;

        // return null if no more tokens
        public JsonToken Read() {
            if (_cache != null) {
                var tmp = _cache;
                _cache = null;
                return tmp;
            }

            return getNextToken();
        }

        public JsonToken Peek() {
            if (_cache != null) return _cache;
            return _cache = getNextToken();
        }

        private JsonToken getNextToken() => _tokens.MoveNext() ? _tokens.Current : null;

        public JsonTokenQueue(string value) {
            _tokens = JsonTokenizer.GetTokens(value).GetEnumerator();
            _cache = null;
        }
    }
}