using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    class FromFileJsonConverter<T> : JsonConverter<T>
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<T>(
                File.ReadAllText(Precondition.NotNull(reader.GetString())), options);
        }

        public override void Write(Utf8JsonWriter writer, T @object, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}