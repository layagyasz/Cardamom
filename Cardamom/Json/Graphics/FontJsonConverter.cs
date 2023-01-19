using Cardamom.Graphics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json.Graphics
{
    public class FontJsonConverter : JsonConverter<Font>
    {
        public override Font Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new Font(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, Font @object, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}