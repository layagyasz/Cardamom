using Cardamom.Graphics.TexturePacking;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json.Graphics.TexturePacking
{
    public class TextureVolumeJsonConverter : JsonConverter<ITextureVolume>
    {
        public override ITextureVolume Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var volume = JsonSerializer.Deserialize<ITextureVolume.IBuilder>(ref reader, options)!.Build();
            if (options.ReferenceHandler != null)
            {
                var resolver = options.ReferenceHandler.CreateResolver();
                foreach (var segment in volume.GetSegments())
                {
                    var referenceId = resolver.GetReference(segment, out bool exists);
                    if (!exists)
                    {
                        resolver.AddReference(referenceId, segment);
                    }
                }
            }
            return volume;
        }

        public override void Write(Utf8JsonWriter writer, ITextureVolume @object, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
