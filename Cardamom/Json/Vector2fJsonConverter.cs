using SFML.System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class Vector2fJsonConverter : JsonConverter<Vector2f>
    {
        public override Vector2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            reader.Read();

            var value = new Vector2f();
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

        public override void Write(Utf8JsonWriter writer, Vector2f @object, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteStringValue(@object.X.ToString());
            writer.WriteStringValue(@object.Y.ToString());
            writer.WriteEndArray();
        }
    }
}