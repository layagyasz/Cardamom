using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class EnumJsonConverter<T> : JsonConverter<T> where T : struct, IConvertible
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string stringValue = Precondition.CheckNotNull(reader.GetString());
            if (!Enum.TryParse(stringValue, ignoreCase: false, out T value) &&
                !Enum.TryParse(stringValue, ignoreCase: true, out value))
            {
                throw new JsonException(
                    string.Format("Unable to convert {0} to Enum {1}.", stringValue, typeof(T)));
            }
            return value;
        }

        public override void Write(Utf8JsonWriter writer, T @object, JsonSerializerOptions options)
        {
            writer.WritePropertyName(Precondition.CheckNotNull(@object.ToString()));
        }
    }
}