using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class FromFileJsonConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return (JsonConverter?)Activator.CreateInstance(
                        typeof(FromFileJsonConverterImpl<>).MakeGenericType(typeToConvert!));
        }

        class FromFileJsonConverterImpl<T> : JsonConverter<T>
        {
            public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<T>(File.ReadAllText(reader.GetString()!), options);
            }

            public override void Write(Utf8JsonWriter writer, T @object, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }
    }
}