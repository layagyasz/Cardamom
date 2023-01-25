using Cardamom.Json.Graphics.TexturePacking;
using System.Text.Json.Serialization;

namespace Cardamom.Graphics.TexturePacking
{
    [JsonConverter(typeof(TextureLibraryJsonConverter))]
    public class TextureLibrary
    {
        public static readonly TextureLibrary Empty = new(Enumerable.Empty<ITextureVolume>());

        private readonly List<ITextureVolume> _volumes;
        private readonly Dictionary<string, TextureSegment> _segments;

        public TextureLibrary(IEnumerable<ITextureVolume> volumes)
        {
            _volumes = volumes.ToList();
            _segments = volumes.SelectMany(x => x.GetSegments()).ToDictionary(x => x.Key, x => x);
        }

        public TextureSegment Get(string key)
        {
            return _segments[key];
        }

        public IEnumerable<TextureSegment> GetSegments()
        {
            return _segments.Values;
        }

        public class Builder
        {
            public List<ITextureVolume.IBuilder> Volumes { get; set; } = new();

            public TextureLibrary Build()
            {
                return new TextureLibrary(Volumes.Select(x => x.Build()));
            }
        }
    }
}
