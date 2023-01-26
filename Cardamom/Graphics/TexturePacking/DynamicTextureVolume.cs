using Cardamom.Json.Collections;
using Cardamom.Mathematics;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Text.Json.Serialization;

namespace Cardamom.Graphics.TexturePacking
{
    public class DynamicTextureVolume : ITextureVolume
    {
        private readonly List<DynamicVariableSizeTexturePage> _pages = new();
        private readonly Dictionary<string, TextureSegment> _segments = new();

        private readonly ITexturePageSupplier _pageSupplier;
        private readonly bool _checkAllPages;

        public DynamicTextureVolume(ITexturePageSupplier pageSupplier, bool checkAllPages)
        {
            _pageSupplier = pageSupplier;
            _checkAllPages = checkAllPages;
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
            if (_checkAllPages)
            {
                foreach (var page in _pages)
                {
                    if (AddToPage(page, key, texture, out var segment))
                    {
                        return segment!;
                    }
                }
            }
            else
            {
                if (AddToPage(_pages.Last(), key, texture, out var segment))
                {
                    return segment!;
                }
            }

            var newPage = _pageSupplier.Get();
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
            if (_checkAllPages)
            {
                foreach (var page in _pages)
                {
                    if (AddToPage(page, key, bitmap, out var segment))
                    {
                        return segment!;
                    }
                }
            }
            else
            {
                if (AddToPage(_pages.Last(), key, bitmap, out var segment))
                {
                    return segment!;
                }
            }

            var newPage = _pageSupplier.Get();
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

        private bool AddToPage(ITexturePage page, string key, Texture texture, out TextureSegment? segment)
        {
            if (page.Add(texture, out var bounds))
            {
                segment = new TextureSegment(key, page.GetTexture(), bounds);
                _segments.Add(key, segment);
                return true;
            }
            segment = null;
            return false;
        }

        private bool AddToPage(ITexturePage page, string key, Bitmap bitmap, out TextureSegment? segment)
        {
            if (page.Add(bitmap, out var bounds))
            {
                segment = new TextureSegment(key, page.GetTexture(), bounds);
                _segments.Add(key, segment);
                return true;
            }
            segment = null;
            return false;
        }

        public class StaticSizeBuilder : ITextureVolume.IBuilder
        {
            public struct DynamicSegment
            {
                public string? Key { get; set; }
                public string? Path { get; set; }
            }

            [JsonConverter(typeof(FromMultipleFileJsonConverter))]
            public List<DynamicSegment> Textures { get; set; } = new();

            public Vector2i Size { get; set; } = new(1024, 1024);
            public Vector2i ElementSize { get; set; } = new();
            public Color4 PageFill { get; set; } = new();
            public Vector2i SegmentPadding { get; set; } = new();

            public ITextureVolume Build()
            {
                DynamicTextureVolume volume =
                    new(new DynamicStaticSizeTexturePage.Supplier(Size, ElementSize, PageFill, SegmentPadding), false);
                foreach (var segment in Textures)
                {
                    volume.Add(segment.Key!, Texture.FromFile(segment.Path!));
                }
                return volume;
            }
        }

        public class VariableSizeBuilder : ITextureVolume.IBuilder
        {
            public struct DynamicSegment
            {
                public string? Key { get; set; }
                public string? Path { get; set; }
            }

            [JsonConverter(typeof(FromMultipleFileJsonConverter))]
            public List<DynamicSegment> Textures { get; set; } = new();

            public IntInterval WidthRange { get; set; } = new(128, 4096);
            public IntInterval HeightRange { get; set; } = new(128, 4096);
            public Color4 PageFill { get; set; } = new();
            public Vector2i SegmentPadding { get; set; } = new();
            public float RowHeightRatio { get; set; } = 1.1f;

            public ITextureVolume Build()
            {
                DynamicTextureVolume volume = 
                    new(
                        new DynamicVariableSizeTexturePage.Supplier(
                            WidthRange, HeightRange, PageFill, SegmentPadding, RowHeightRatio), true);
                foreach (var segment in Textures)
                {
                    volume.Add(segment.Key!, Texture.FromFile(segment.Path!));
                }
                return volume;
            }
        }
    }
}
