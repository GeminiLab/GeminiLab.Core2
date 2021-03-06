using System;
using GeminiLab.Core2.IO;
using GeminiLab.Core2.Text;

namespace GeminiLab.Core2.Markup.Json {
    public sealed class JsonString : JsonValue, IEquatable<JsonString> {
        public string Value { get; }

        public JsonString(string value) {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        internal override void Stringify(JsonStringifyOption config, IndentedWriter iw) {
            iw.Write('\"');
            iw.Write(config.HasFlag(JsonStringifyOption.AsciiOnly) ? EscapeSequenceConverter.EncodeToAscii(Value) : EscapeSequenceConverter.Encode(Value));
            iw.Write('\"');
        }

        public bool Equals(JsonString other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj switch {
                JsonString s => Equals(s),
                string s => s == Value,
                _ => false,
            };
        }

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(JsonString a, JsonString b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(JsonString a, JsonString b) => !(a == b);

        public static implicit operator string(JsonString str) => str.Value;
        public static implicit operator JsonString(string str) => new JsonString(str);
    }
}