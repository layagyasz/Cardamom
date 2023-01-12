using OpenTK.Mathematics;

namespace Cardamom.Graphics.TexturePacking
{
    public class StaticTexturePage : ITexturePage, ITextureVolume
    {
        private readonly Texture _texture;
        private readonly Dictionary<string, TextureSegment> _segments;

        public StaticTexturePage(Texture texture, IEnumerable<TextureSegment> segments)
        {
            _texture = texture;
            _segments = segments.ToDictionary(x => x.Key, x => x);
        }

        public IEnumerable<TextureSegment> GetSegments()
        {
            return _segments.Values;
        }

        public Texture GetTexture()
        {
            return _texture; 
        }

        public IEnumerable<Texture> GetTextures()
        {
            yield return _texture;
        }

        public bool Add(Texture texture, out Box2i bounds)
        {
            throw new InvalidOperationException();
        }

        public bool Add(Bitmap bitmap, out Box2i bounds)
        {
            throw new InvalidOperationException();
        }

        public TextureSegment Add(string key, Texture texture)
        {
            throw new InvalidOperationException();
        }

        public TextureSegment Add(string key, Bitmap bitmap)
        {
            throw new InvalidOperationException();
        }

        public TextureSegment Get(string key)
        {
            return _segments[key];
        }

        public class Builder : ITextureVolume.IBuilder
        {
            public struct StaticSegment
            {
                public string? Key { get; set; }
                public Vector2i TopLeft { get; set; }
                public Vector2i Size { get; set; }

                public TextureSegment ToSegment(Texture texture)
                {
                    return new TextureSegment(Key!, texture, new(TopLeft, Size));
                }
            }

            public string? TexturePath { get; set; }
            public List<StaticSegment>? Segments { get; set; }

            public ITextureVolume Build()
            {
                var texture = Texture.FromFile(TexturePath!);
                return new StaticTexturePage(texture, Segments!.Select(x => x.ToSegment(texture)));
            }
        }
    }
}
