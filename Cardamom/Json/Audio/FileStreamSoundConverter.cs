using Cardamom.Audio;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json.Audio
{
    public class FileStreamSoundConverter : JsonConverter<ISound>
    {
        public override ISound Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return FileStreamSound.FromFile(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, ISound @object, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
