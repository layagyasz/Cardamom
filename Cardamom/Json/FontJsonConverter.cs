using SFML.Graphics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class FontJsonConverter : JsonConverter<Font>
    {
        public override Font Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string stringValue = Precondition.NotNull(reader.GetString());
            return new Font(stringValue);
        }

        public override void Write(Utf8JsonWriter writer, Font @object, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
