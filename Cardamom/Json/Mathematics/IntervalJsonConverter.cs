using Cardamom.Mathematics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json.Mathematics
{
    public class IntervalJsonConverter : JsonConverter<Interval>
    {
        public override Interval Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            reader.Read();

            var value = new Interval();
            value.Minimum = reader.GetSingle();
            reader.Read();
            value.Maximum = reader.GetSingle();
            reader.Read();

            if (reader.TokenType != JsonTokenType.EndArray)
            {
                throw new JsonException();
            }
            return value;
        }

        public override void Write(Utf8JsonWriter writer, Interval @object, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteStringValue(@object.Minimum.ToString());
            writer.WriteStringValue(@object.Maximum.ToString());
            writer.WriteEndArray();
        }
    }
}
