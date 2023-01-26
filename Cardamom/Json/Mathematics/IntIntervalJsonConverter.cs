using Cardamom.Mathematics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json.Mathematics
{
    public class IntIntervalJsonConverter : JsonConverter<IntInterval>
    {
        public override IntInterval Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            reader.Read();

            var value = new IntInterval();
            value.Minimum = reader.GetInt32();
            reader.Read();
            value.Maximum = reader.GetInt32();
            reader.Read();

            if (reader.TokenType != JsonTokenType.EndArray)
            {
                throw new JsonException();
            }
            return value;
        }

        public override void Write(Utf8JsonWriter writer, IntInterval @object, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(@object.Minimum);
            writer.WriteNumberValue(@object.Maximum);
            writer.WriteEndArray();
        }
    }
}
