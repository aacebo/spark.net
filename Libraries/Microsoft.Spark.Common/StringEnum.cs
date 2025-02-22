using System.Text.Json;

namespace Microsoft.Spark.Common;

[System.Text.Json.Serialization.JsonConverter(typeof(JsonConverter<StringEnum>))]
public class StringEnum(string value) : ICloneable, IComparable, IComparable<string>, IEquatable<string>
{
    public string Value { get; set; } = value;

    public object Clone() => new StringEnum(Value);
    public int CompareTo(object? value) => Value.CompareTo(value);
    public int CompareTo(string? value) => Value.CompareTo(value);
    public bool Equals(string? value) => Value.Equals(value);
    public override bool Equals(object? value) => Value.Equals(value);
    public override string ToString() => Value;
    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(StringEnum? a, StringEnum? b) => a?.Value == b?.Value;
    public static bool operator !=(StringEnum? a, StringEnum? b) => a?.Value != b?.Value;

    public class JsonConverter<TStringEnum> : System.Text.Json.Serialization.JsonConverter<TStringEnum>
        where TStringEnum : StringEnum
    {
        public override TStringEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            if (value == null)
            {
                throw new JsonException("value must not be null");
            }

            var res = Activator.CreateInstance(
                typeof(TStringEnum),
                [value]
            );

            if (res == null)
            {
                throw new JsonException($"could not create instance of '{typeof(TStringEnum)}'");
            }

            return (TStringEnum)res;
        }

        public override void Write(Utf8JsonWriter writer, TStringEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}