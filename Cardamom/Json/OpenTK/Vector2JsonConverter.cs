using OpenTK.Mathematics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json.OpenTK
{
    public class Vector2JsonConverter : JsonConverter<Vector2>
    {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            reader.Read();

            var value = new Vector2();
            value.X = reader.GetSingle();
            reader.Read();
            value.Y = reader.GetSingle();
            reader.Read();

            if (reader.TokenType != JsonTokenType.EndArray)
            {
                throw new JsonException();
            }
            return value;
        }

        public override void Write(Utf8JsonWriter writer, Vector2 @object, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(@object.X);
            writer.WriteNumberValue(@object.Y);
            writer.WriteEndArray();
        }
    }
}