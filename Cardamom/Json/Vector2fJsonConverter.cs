using SFML.System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class Vector2fJsonConverter : JsonConverter<Vector2f>
    {
        public override Vector2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? def = reader.GetString();
            if (def != null)
            {
                string[] code = def.Split(',');
                if (code.Length == 2)
                {
                    return new Vector2f(float.Parse(code[0]), float.Parse(code[1]));
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Vector2f @object, JsonSerializerOptions options)
        {
            writer.WriteStringValue(string.Format("{0},{1}", @object.X, @object.Y));
        }
    }
}