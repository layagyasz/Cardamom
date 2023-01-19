using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class GenericJsonConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return (JsonConverter?)Activator.CreateInstance(
                typeof(GenericJsonConverterImpl<>).MakeGenericType(typeToConvert));
        }

        class GenericJsonConverterImpl<T> : JsonConverter<T>
        {
            public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException();
                }
                reader.Read();
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }
                if (reader.GetString() != "Type")
                {
                    throw new JsonException();
                }
                reader.Read();
                Type type = Type.GetType(reader.GetString())!;
                reader.Read();

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }
                if (reader.GetString() != "Object")
                {
                    throw new JsonException();
                }
                reader.Read();

                return (T?)JsonSerializer.Deserialize(ref reader, type, options);
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteString("Type", value!.GetType().ToString());
                writer.WritePropertyName("Object");
                JsonSerializer.Serialize(writer, value, options);
                writer.WriteEndObject();
            }
        }
    }
}
