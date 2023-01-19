using Cardamom.Graphics.TexturePacking;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json.Graphics.TexturePacking
{
    public class TextureLibraryJsonConverter : JsonConverter<TextureLibrary>
    {
        public override TextureLibrary Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var library = JsonSerializer.Deserialize<TextureLibrary.Builder>(ref reader, options)!.Build();
            if (options.ReferenceHandler != null)
            {
                var resolver = options.ReferenceHandler.CreateResolver();
                foreach (var segment in library.GetSegments())
                {
                    var referenceId = resolver.GetReference(segment, out bool exists);
                    if (!exists)
                    {
                        resolver.AddReference(referenceId, segment);
                    }
                }
            }
            return library;
        }

        public override void Write(Utf8JsonWriter writer, TextureLibrary @object, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
