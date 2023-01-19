using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json.Collections
{
    public class ListDictionaryJsonConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert
                .GetInterfaces()
                .Any(x => x.IsGenericType && typeof(IDictionary<,>).IsAssignableFrom(x.GetGenericTypeDefinition()));
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            foreach (var @interface in typeToConvert.GetInterfaces())
            {
                if (@interface.IsGenericType
                    && typeof(IDictionary<,>).IsAssignableFrom(@interface.GetGenericTypeDefinition()))
                {
                    var args = @interface.GetGenericArguments();
                    var converterArgs = new Type[] { typeToConvert, args[0], args[1] };
                    return (JsonConverter?)Activator.CreateInstance(
                        typeof(DictionaryListJsonConverterImpl<,,>).MakeGenericType(converterArgs));
                }
            }
            throw new JsonException();
        }

        class DictionaryListJsonConverterImpl<TDict, TKey, TValue>
            : JsonConverter<TDict> where TDict : IDictionary<TKey, TValue>
        {
            public override TDict? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException();
                }

                var dict = (TDict)Activator.CreateInstance(typeToConvert)!;
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        return dict;
                    }

                    var kvp = JsonSerializer.Deserialize<KeyValuePair<TKey, TValue>>(ref reader, options);
                    dict.Add(kvp);
                }
                throw new JsonException();
            }

            public override void Write(Utf8JsonWriter writer, TDict value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value.ToList(), options);
            }
        }
    }
}
