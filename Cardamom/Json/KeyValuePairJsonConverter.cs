using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class KeyValuePairJsonConverter<TCollection, TKey, TValue>
        : JsonConverter<TCollection> where TCollection : ICollection<KeyValuePair<TKey, TValue>>
    {
        public JsonConverter<TKey>? KeyConverter { get; }
        public JsonConverter<TValue>? ValueConverter { get; }

        public KeyValuePairJsonConverter() : this(null, null) { }

        public KeyValuePairJsonConverter(JsonConverter<TKey>? keyConverter, JsonConverter<TValue>? valueConverter)
        {
            KeyConverter = keyConverter;
            ValueConverter = valueConverter;
        }

        public override TCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            TCollection dict = 
                (TCollection)typeof(TCollection).GetConstructor(Array.Empty<Type>())!.Invoke(Array.Empty<object>());

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return dict;
                }
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                TKey key;
                JsonConverter<TKey> keyConverter =
                    KeyConverter ?? (JsonConverter<TKey>)options.GetConverter(typeof(TKey));
                if (keyConverter != null)
                {
                    key = keyConverter.Read(ref reader, typeof(TKey), options)!;
                }
                else
                {
                    key = JsonSerializer.Deserialize<TKey>(ref reader, options)!;
                }

                TValue value;
                JsonConverter<TValue> valueConverter =
                    ValueConverter ?? (JsonConverter<TValue>)options.GetConverter(typeof(TValue));
                reader.Read();
                if (valueConverter != null)
                {
                    value = valueConverter.Read(ref reader, typeof(TValue), options)!;
                }
                else
                {
                    value = JsonSerializer.Deserialize<TValue>(ref reader, options)!;
                }

                dict.Add(new KeyValuePair<TKey, TValue>(key, value));
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, TCollection @object, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            JsonConverter<TValue> valueConverter = (JsonConverter<TValue>)options.GetConverter(typeof(TValue));
            foreach (KeyValuePair<TKey, TValue> kvp in @object)
            {
                writer.WritePropertyName(kvp.Key!.ToString()!);

                if (valueConverter != null)
                {
                    valueConverter.Write(writer, kvp.Value, options);
                }
                else
                {
                    JsonSerializer.Serialize(writer, kvp.Value, options);
                }
            }

            writer.WriteEndObject();
        }
    }
}