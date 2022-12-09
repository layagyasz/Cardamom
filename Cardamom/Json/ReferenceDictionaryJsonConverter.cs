using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class ReferenceDictionaryJsonConverter : JsonConverterFactory
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
                        typeof(ReferenceDictionaryJsonConverterImpl<,,>).MakeGenericType(converterArgs));
                }
            }
            throw new JsonException();
        }

        internal class ReferenceDictionaryJsonConverterImpl<TDict, TKey, TValue>
            : JsonConverter<TDict> where TDict : IDictionary<TKey, TValue>
        {
            public override TDict Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException();
                }

                var dict = (TDict)Activator.CreateInstance(typeToConvert)!;
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        return dict;
                    }

                    string key = reader.GetString()!;
                    reader.Read();
                    TValue value = JsonSerializer.Deserialize<TValue>(ref reader, options)!;

                    dict.Add((TKey)options.ReferenceHandler!.CreateResolver().ResolveReference(key), value);
                }

                throw new JsonException();
            }

            public override void Write(Utf8JsonWriter writer, TDict @object, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                foreach (KeyValuePair<TKey, TValue> kvp in @object)
                {
                    writer.WritePropertyName(
                        options.ReferenceHandler!.CreateResolver().GetReference(kvp.Key!, out bool _));
                    JsonSerializer.Serialize(writer, kvp.Value, options);
                }
                writer.WriteEndObject();
            }
        }
    }
}
