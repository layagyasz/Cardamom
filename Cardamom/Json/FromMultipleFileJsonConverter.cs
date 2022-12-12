using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class FromMultipleFileJsonConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert
                .GetInterfaces()
                .Any(x => x.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(x.GetGenericTypeDefinition()));
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            foreach (var @interface in typeToConvert.GetInterfaces())
            {
                if (@interface.IsGenericType
                    && typeof(IEnumerable<>).IsAssignableFrom(@interface.GetGenericTypeDefinition()))
                {
                    var args = @interface.GetGenericArguments();
                    var converterArgs = new Type[] { typeToConvert, args[0] };
                    return (JsonConverter?)Activator.CreateInstance(
                        typeof(FromMultipleFileJsonConverterImpl<,>).MakeGenericType(converterArgs));
                }
            }
            throw new JsonException();
        }

        class FromMultipleFileJsonConverterImpl<TCollection, TValue> 
            : JsonConverter<TCollection> where TCollection : ICollection<TValue>
        {
            public override TCollection? Read(
                ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var result = (TCollection)Activator.CreateInstance(typeToConvert)!;
                var patterns = JsonSerializer.Deserialize<List<string>>(ref reader, options)!;
                var files = new HashSet<string>();
                foreach (var pattern in patterns)
                {
                    foreach (var file in Directory.EnumerateFiles(string.Empty, pattern, SearchOption.AllDirectories))
                    {
                        files.Add(file);
                    }
                }
                foreach (var file in files)
                {
                    foreach (var value in JsonSerializer.Deserialize<TCollection>(File.ReadAllText(file), options)!)
                    {
                        result.Add(value);
                    }
                }
                return result;
            }

            public override void Write(Utf8JsonWriter writer, TCollection @object, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }
    }
}
