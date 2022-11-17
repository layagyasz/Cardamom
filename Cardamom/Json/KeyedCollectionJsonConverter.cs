using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class KeyedCollectionJsonConverter<TCollection, T> 
        : CollectionJsonConverter<TCollection, T> where T : IKeyed where TCollection : ICollection<T>
    {
        private readonly Dictionary<string, IKeyed> _objects;

        public KeyedCollectionJsonConverter(Dictionary<string, IKeyed> objects)
        {
            _objects = objects;
        }

        public override TCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            TCollection collect =
                (TCollection)Precondition.NotNull(typeof(TCollection).GetConstructor(Array.Empty<Type>()))
                                         .Invoke(Array.Empty<object>());

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return collect;
                }

                T value;
                JsonConverter<T> converter = Converter ?? (JsonConverter<T>)options.GetConverter(typeof(T));
                if (converter != null)
                {
                    value = Precondition.NotNull(converter.Read(ref reader, typeof(T), options));
                }
                else
                {
                    value = Precondition.NotNull(JsonSerializer.Deserialize<T>(ref reader, options));
                }

                _objects.Add(Precondition.NotNull(value).Key, value);
                collect.Add(value);
            }

            throw new JsonException();
        }
    }
}
