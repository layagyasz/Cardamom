using OpenTK.Mathematics;
using System.Text.Json.Serialization;

namespace Cardamom.Graphics
{
    public class TextureLibrary
    {
        public static readonly TextureLibrary Empty = 
            new(Enumerable.Empty<Texture>(), Enumerable.Empty<TextureSegment>());

        private readonly List<Texture> _textures;
        private readonly Dictionary<string, TextureSegment> _segments;

        public TextureLibrary(IEnumerable<Texture> textures, IEnumerable<TextureSegment> segments)
        {
            _textures = textures.ToList();
            _segments = segments.ToDictionary(x => x.Key, x => x);
        }

        public TextureSegment Get(string key)
        {
            return _segments[key];
        }

        public IEnumerable<Texture> GetTextures()
        {
            return _textures;
        }

        public IEnumerable<TextureSegment> GetSegments()
        {
            return _segments.Values;
        }

        [JsonDerivedType(typeof(CompositeBuilder), "composite")]
        [JsonDerivedType(typeof(StaticBuilder), "static")]
        public interface IBuilder
        {
            TextureLibrary Build();
        }

        public class CompositeBuilder : IBuilder
        {
            public List<IBuilder>? Volumes { get; set; }

            public TextureLibrary Build()
            {
                var volumes = Volumes!.Select(x => x.Build());
                return new TextureLibrary(
                    volumes.SelectMany(x => x.GetTextures()), volumes.SelectMany(x => x.GetSegments()));
            }
        }

        public class StaticBuilder : IBuilder
        {
            public struct StaticSegment
            {
                public string? Key { get; set; }
                public Vector2 TopLeft { get; set; }
                public Vector2 Size { get; set; }

                public TextureSegment ToSegment(Texture texture)
                {
                    return new TextureSegment(Key!, texture, new(TopLeft / texture.Size, Size / texture.Size));
                }
            }

            public string? TexturePath { get; set; }
            public List<StaticSegment>? Segments { get; set; }

            public TextureLibrary Build()
            {
                var texture = Texture.FromFile(TexturePath!);
                return new TextureLibrary(Enumerable.Repeat(texture, 1), Segments!.Select(x => x.ToSegment(texture)));
            }
        }
    }
}
