using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class LibraryJsonConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert
                .GetInterfaces()
                .Any(
                    x => 
                        x.IsGenericType 
                        && typeof(IDictionary<,>).IsAssignableFrom(x.GetGenericTypeDefinition())
                        && x.GetGenericArguments()[0] == typeof(string));
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            foreach (var @interface in typeToConvert.GetInterfaces())
            {
                if (@interface.IsGenericType
                    && typeof(IDictionary<,>).IsAssignableFrom(@interface.GetGenericTypeDefinition()))
                {
                    var args = @interface.GetGenericArguments();
                    Precondition.Check(args[0] == typeof(string));
                    var converterArgs = new Type[] { typeToConvert, args[1] };
                    return (JsonConverter?)Activator.CreateInstance(
                        typeof(LibraryJsonConverterImpl<,>).MakeGenericType(converterArgs));
                }
            }
            throw new JsonException();
        }

        class LibraryJsonConverterImpl<TDict, TValue>
            : JsonConverter<TDict> where TDict : IDictionary<string, TValue>
        {
            public override TDict? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException();
                }

                var referenceResolver = options.ReferenceHandler!.CreateResolver();
                var dict = (TDict)Activator.CreateInstance(typeToConvert)!;
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        return dict;
                    }

                    var value = JsonSerializer.Deserialize<TValue>(ref reader, options)!;
                    dict.Add(referenceResolver.GetReference(value, out bool _), value);
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
