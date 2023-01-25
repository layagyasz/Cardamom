using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json.Collections
{
    public class ReferenceCollectionJsonConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert
                .GetInterfaces()
                .Any(x => x.IsGenericType && typeof(ICollection<>).IsAssignableFrom(x.GetGenericTypeDefinition()));
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            foreach (var @interface in typeToConvert.GetInterfaces())
            {
                if (@interface.IsGenericType
                    && typeof(ICollection<>).IsAssignableFrom(@interface.GetGenericTypeDefinition()))
                {
                    var args = @interface.GetGenericArguments();
                    var converterArgs = new Type[] { typeToConvert, args[0] };
                    return (JsonConverter?)Activator.CreateInstance(
                        typeof(ReferenceCollectionJsonConverterImpl<,>).MakeGenericType(converterArgs));
                }
            }
            throw new JsonException();
        }

        internal class ReferenceCollectionJsonConverterImpl<TCollection, TElement>
            : JsonConverter<TCollection> where TCollection : ICollection<TElement>
        {
            public override TCollection Read(
                ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException();
                }

                var collect = (TCollection)Activator.CreateInstance(typeToConvert)!;
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        return collect;
                    }
                    collect.Add(
                        (TElement)options.ReferenceHandler!.CreateResolver().ResolveReference(reader.GetString()!));
                }

                throw new JsonException();
            }

            public override void Write(Utf8JsonWriter writer, TCollection @object, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                foreach (var element in @object)
                {
                    writer.WritePropertyName(
                        options.ReferenceHandler!.CreateResolver().GetReference(element!, out bool _));
                }
                writer.WriteEndArray();
            }
        }
    }
}
