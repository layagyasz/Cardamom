using OpenTK.Mathematics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json.OpenTK
{
    public class Vector2iJsonConverter : JsonConverter<Vector2i>
    {
        public override Vector2i Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            reader.Read();

            var value = new Vector2i();
            value.X = reader.GetInt32();
            reader.Read();
            value.Y = reader.GetInt32();
            reader.Read();

            if (reader.TokenType != JsonTokenType.EndArray)
            {
                throw new JsonException();
            }
            return value;
        }

        public override void Write(Utf8JsonWriter writer, Vector2i @object, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteStringValue(@object.X.ToString());
            writer.WriteStringValue(@object.Y.ToString());
            writer.WriteEndArray();
        }
    }
}
