using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class CollectionJsonConverter<TCollection, T> 
        : JsonConverter<TCollection> where TCollection : ICollection<T>
    {
        public JsonConverter<T>? Converter { get; }

        public CollectionJsonConverter() : this(null) { }

        public CollectionJsonConverter(JsonConverter<T>? converter)
        {
            Converter = converter;
        }

        public override TCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            TCollection collect = 
                (TCollection)typeof(TCollection).GetConstructor(Array.Empty<Type>())!.Invoke(Array.Empty<object>());

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return collect;
                }

                T? value;
                JsonConverter<T> converter = Converter ?? (JsonConverter<T>)options.GetConverter(typeof(T));
                if (converter != null)
                {
                    value = converter.Read(ref reader, typeof(T), options);
                }
                else
                {
                    value = JsonSerializer.Deserialize<T>(ref reader, options);
                }

                collect.Add(value!);
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, TCollection @object, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            JsonConverter<T> valueConverter = (JsonConverter<T>)options.GetConverter(typeof(T));
            foreach (var item in @object)
            {
                if (valueConverter != null)
                {
                    valueConverter.Write(writer, item, options);
                }
                else
                {
                    JsonSerializer.Serialize(writer, item, options);
                }
            }
            writer.WriteEndArray();
        }
    }
}