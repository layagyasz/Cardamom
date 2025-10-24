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
                string? reference = null;
                if (reader.GetString() != "Type")
                {
                    if (reader.GetString() == "$ref")
                    {
                        reader.Read();
                        reference = reader.GetString();
                        reader.Read();
                        return (T?)options.ReferenceHandler?.CreateResolver().ResolveReference(reference!);
                    }
                    else if (reader.GetString() == "$id")
                    {
                        reader.Read();
                        reference = reader.GetString();
                        reader.Read();
                    }
                    if (reader.GetString() != "Type")
                    {
                        throw new JsonException();
                    }
                }
                reader.Read();
                Type type = Type.GetType(reader.GetString()!)!;
                reader.Read();

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }
                if (reader.GetString() != "Object")
                {
                    throw new JsonException();
                }

                var result = (T?)JsonSerializer.Deserialize(ref reader, type, options);
                reader.Read();

                if (reference != null)
                {
                    options.ReferenceHandler?.CreateResolver().AddReference(reference!, result!);
                }

                return result;
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
