using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class ReferenceJsonConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return (JsonConverter?)Activator.CreateInstance(
                typeof(ReferenceJsonConverterImpl<>).MakeGenericType(typeToConvert));
        }

        internal class ReferenceJsonConverterImpl<T> : JsonConverter<T>
        {
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException();
                }
                return (T)options.ReferenceHandler!.CreateResolver().ResolveReference(reader.GetString()!);
            }

            public override void Write(Utf8JsonWriter writer, T @object, JsonSerializerOptions options)
            {
                writer.WritePropertyName(
                    options.ReferenceHandler!.CreateResolver().GetReference(@object!, out bool _));
            }
        }
    }
}
