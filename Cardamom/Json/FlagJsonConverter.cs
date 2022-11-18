using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class FlagJsonConverter<T> : JsonConverter<T> where T : Enum
    {
        private readonly CollectionJsonConverter<List<T>, T> _internalConverter = new();

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<T> values = _internalConverter.Read(ref reader, typeToConvert, options);
            return (T)(object)values.Aggregate(0, (x, y) => x + (int)(object)y);
        }

        public override void Write(Utf8JsonWriter writer, T @object, JsonSerializerOptions options)
        {
            var values = Enum.GetValues(@object.GetType()).Cast<Enum>().Where(@object.HasFlag).Cast<T>().ToList();
            _internalConverter.Write(writer, values, options);
        }
    }
}
