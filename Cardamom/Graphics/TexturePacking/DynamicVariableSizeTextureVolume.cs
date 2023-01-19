using Cardamom.Json.Collections;
using OpenTK.Mathematics;
using System.Text.Json.Serialization;

namespace Cardamom.Graphics.TexturePacking
{
    public class DynamicVariableSizeTextureVolume : ITextureVolume
    {
        private readonly List<DynamicVariableSizeTexturePage> _pages = new();
        private readonly Dictionary<string, TextureSegment> _segments = new();

        private readonly Vector2i _initialPageSize;
        private readonly Color4 _pageFill;
        private readonly Vector2i _segmentPadding;
        private readonly float _rowHeightRatio;

        public DynamicVariableSizeTextureVolume(
            Vector2i initialPageSize, Color4 pageFill, Vector2i segmentPadding, float rowHeightRatio)
        {
            _initialPageSize = initialPageSize;
            _pageFill = pageFill;
            _segmentPadding = segmentPadding;
            _rowHeightRatio = rowHeightRatio;
        }

        public IEnumerable<TextureSegment> GetSegments()
        {
            return _segments.Values;
        }

        public IEnumerable<Texture> GetTextures()
        {
            return _pages.Select(x => x.GetTexture());
        }

        public TextureSegment Add(string key, Texture texture)
        {
            Box2i bounds;
            foreach (var page in _pages)
            {
                if (page.Add(texture, out bounds))
                {
                    var segment = new TextureSegment(key, page.GetTexture(), bounds);
                    _segments.Add(key, segment);
                    return segment;
                }
            }

            var newPage = CreatePage();
            if (newPage.Add(texture, out bounds))
            {
                var segment = new TextureSegment(key, newPage.GetTexture(), bounds);
                _segments.Add(key, segment);
                return segment;
            }
            else
            {
                throw new ArgumentException("Input too large to fit on texture page.");
            }
        }

        public TextureSegment Add(string key, Bitmap bitmap)
        {
            Box2i bounds;
            foreach (var page in _pages)
            {
                if (page.Add(bitmap, out bounds))
                {
                    var segment = new TextureSegment(key, page.GetTexture(), bounds);
                    _segments.Add(key, segment);
                    return segment;
                }
            }

            var newPage = CreatePage();
            if (newPage.Add(bitmap, out bounds))
            {
                var segment = new TextureSegment(key, newPage.GetTexture(), bounds);
                _segments.Add(key, segment);
                return segment;
            }
            else
            {
                throw new ArgumentException("Input too large to fit on texture page.");
            }
        }

        public TextureSegment Get(string key)
        {
            return _segments[key];
        }

        private ITexturePage CreatePage()
        {
            var newPage =
                new DynamicVariableSizeTexturePage(_initialPageSize, _pageFill, _segmentPadding, _rowHeightRatio);
            _pages.Add(newPage);
            return newPage;
        }

        public class Builder : ITextureVolume.IBuilder
        {
            public struct DynamicSegment
            {
                public string? Key { get; set; }
                public string? Path { get; set; }
            }

            [JsonConverter(typeof(FromMultipleFileJsonConverter))]
            public List<DynamicSegment> Textures { get; set; } = new();

            public Vector2i InitialPageSize { get; set; } = new(128, 128);
            public Color4 PageFill { get; set; } = new();
            public Vector2i SegmentPadding { get; set; } = new();
            public float RowHeightRatio { get; set; } = 1.1f;

            public ITextureVolume Build()
            {
                DynamicVariableSizeTextureVolume volume = 
                    new(InitialPageSize, PageFill, SegmentPadding, RowHeightRatio);
                foreach (var segment in Textures)
                {
                    volume.Add(segment.Key!, Texture.FromFile(segment.Path!));
                }
                return volume;
            }
        }
    }
}
