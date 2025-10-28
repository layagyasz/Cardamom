using Cardamom.Json.Collections;
using Cardamom.Mathematics;
using Cardamom.Utils.IO;
using OpenTK.Mathematics;
using System.Text.Json.Serialization;

namespace Cardamom.Graphics.TexturePacking
{
    public class DynamicTextureVolume : ManagedResource, ITextureVolume
    {
        private List<ITexturePage>? _pages = new();
        private Dictionary<string, TextureSegment>? _segments = new();

        private readonly ITexturePageSupplier _pageSupplier;
        private readonly bool _checkAllPages;

        public DynamicTextureVolume(ITexturePageSupplier pageSupplier, bool checkAllPages)
        {
            _pageSupplier = pageSupplier;
            _checkAllPages = checkAllPages;
        }

        protected override void DisposeImpl()
        {
            foreach (var page in _pages!)
            {
                page.Dispose();
            }
            _pages = null;
            _segments = null;
        }

        public IEnumerable<TextureSegment> GetSegments()
        {
            return _segments!.Values;
        }

        public IEnumerable<Texture> GetTextures()
        {
            return _pages!.Select(x => x.GetTexture());
        }

        public TextureSegment Add(string key, Texture texture)
        {
            if (_checkAllPages)
            {
                foreach (var page in _pages!)
                {
                    if (AddToPage(page, key, texture, out var segment))
                    {
                        return segment!;
                    }
                }
            }
            else
            {
                if (_pages!.Count > 0 && AddToPage(_pages!.Last(), key, texture, out var segment))
                {
                    return segment!;
                }
            }

            var newPage = _pageSupplier.Get();
            if (newPage.Add(texture, out Box2i bounds))
            {
                var segment = new TextureSegment(key, newPage.GetTexture(), bounds);
                _segments!.Add(key, segment);
                _pages.Add(newPage);
                return segment;
            }
            else
            {
                throw new ArgumentException("Input too large to fit on texture page.");
            }
        }

        public TextureSegment Add(string key, Bitmap bitmap)
        {
            if (_checkAllPages)
            {
                foreach (var page in _pages!)
                {
                    if (AddToPage(page, key, bitmap, out var segment))
                    {
                        return segment!;
                    }
                }
            }
            else
            {
                if (_pages!.Count > 0 && AddToPage(_pages!.Last(), key, bitmap, out var segment))
                {
                    return segment!;
                }
            }

            var newPage = _pageSupplier.Get();
            if (newPage.Add(bitmap, out Box2i bounds))
            {
                var segment = new TextureSegment(key, newPage.GetTexture(), bounds);
                _segments!.Add(key, segment);
                _pages!.Add(newPage);
                return segment;
            }
            else
            {
                throw new ArgumentException("Input too large to fit on texture page.");
            }
        }

        public TextureSegment Get(string key)
        {
            return _segments![key];
        }

        private bool AddToPage(ITexturePage page, string key, Texture texture, out TextureSegment? segment)
        {
            if (page.Add(texture, out var bounds))
            {
                segment = new TextureSegment(key, page.GetTexture(), bounds);
                _segments!.Add(key, segment);
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
                _segments!.Add(key, segment);
                return true;
            }
            segment = null;
            return false;
        }

        public class TextureSet
        {
            public record struct DynamicSegment(string Key, string Path);

            public string Prefix { get; set; } = string.Empty;

            [JsonConverter(typeof(FromMultipleFileJsonConverter))]
            public List<DynamicSegment> Explicit { get; set; } = new();
            public List<string> Implicit { get; set; } = new();

            public IEnumerable<DynamicSegment> GetSegments()
            {
                foreach (var segment in Explicit)
                {
                    yield return new()
                    { 
                        Key = Prefix + segment.Key,
                        Path = segment.Path 
                    };
                }
                foreach (var file in Implicit.SelectMany(Glob.GetFiles))
                {
                    yield return new DynamicSegment()
                    {
                        Key = Prefix + Path.GetFileNameWithoutExtension(file),
                        Path = file
                    };
                }
            }
        }

        public class StaticSizeBuilder : ITextureVolume.IBuilder
        {
            public TextureSet? Textures { get; set; }
            public Vector2i Size { get; set; } = new(1024, 1024);
            public Vector2i ElementSize { get; set; }
            public Color4 PageFill { get; set; }
            public Vector2i SegmentPadding { get; set; }
            public Texture.Parameters TextureParameters { get; set; } = new();

            public ITextureVolume Build()
            {
                DynamicTextureVolume volume =
                    new(
                        new DynamicStaticSizeTexturePage.Supplier(
                            Size, ElementSize, PageFill, SegmentPadding, TextureParameters), false);
                foreach (var segment in Textures!.GetSegments())
                {
                    var bitmap = Bitmap.FromFile(segment.Path);
                    volume.Add(segment.Key!, bitmap);
                }
                return volume;
            }
        }

        public class VariableSizeBuilder : ITextureVolume.IBuilder
        {
            public TextureSet? Textures { get; set; }
            public IntInterval WidthRange { get; set; } = new(128, 4096);
            public IntInterval HeightRange { get; set; } = new(128, 4096);
            public Color4 PageFill { get; set; } = new();
            public Vector2i SegmentPadding { get; set; } = new();
            public float RowHeightRatio { get; set; } = 1.1f;
            public Texture.Parameters TextureParameters { get; set; } = new();

            public ITextureVolume Build()
            {
                DynamicTextureVolume volume = 
                    new(
                        new DynamicVariableSizeTexturePage.Supplier(
                            WidthRange, HeightRange, PageFill, SegmentPadding, RowHeightRatio, TextureParameters), 
                        true);
                foreach (var segment in Textures!.GetSegments())
                {
                    var bitmap = Bitmap.FromFile(segment.Path);
                    volume.Add(segment.Key!, bitmap);
                }
                return volume;
            }
        }
    }
}
