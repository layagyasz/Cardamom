using OpenTK.Mathematics;
using OpenTK.Platform.Windows;
using SharpFont;
using System.Net.Security;

namespace Cardamom.Graphics
{
    public class Font
    {
        private readonly Face _face;
        private readonly Dictionary<uint, TexturePage> _pages = new();
        private readonly Dictionary<CompositeKey<uint, uint>, Glyph> _glyphs = new();

        public Font(string path)
        {
            var library = new Library();
            _face = new Face(library, path);
        }

        public Glyph GetOrLoadGlyph(uint code, uint characterSize)
        {
            if (_glyphs.TryGetValue(CompositeKey<uint, uint>.Create(code, characterSize), out Glyph glyph))
            {
                return glyph;
            }
            return LoadGlyph(code, characterSize);
        }

        private Glyph LoadGlyph(uint code, uint characterSize)
        {
            _face.SetCharSize(0, (float)characterSize, 0, 96);
            _face.LoadGlyph(_face.GetCharIndex(code), LoadFlags.Default, LoadTarget.Normal);
            _face.Glyph.RenderGlyph(RenderMode.Normal);

            var page = GetOrLoadPage(characterSize);
            var bitmap = _face.Glyph.Bitmap;
            var size = new Vector2i(bitmap.Width, bitmap.Rows);
            var textureView = page.Add(size, TranslateBitmap(bitmap));

            var glyph =
                new Glyph()
                {
                    Advance = (float)_face.Glyph.Advance.X,
                    Bounds = new(new(-_face.Glyph.BitmapLeft, -_face.Glyph.BitmapTop), size),
                    TextureView = textureView
                };
            _glyphs.Add(CompositeKey<uint, uint>.Create(code, characterSize), glyph);
            return glyph;
        }

        public float GetLineSpacing(uint characterSize)
        {
            _face.SetCharSize(0, (float)characterSize, 0, 96);
            return (float)_face.Size.Metrics.Height;
        }

        private TexturePage GetOrLoadPage(uint characterSize)
        {
            if (_pages.TryGetValue(characterSize, out TexturePage? page))
            {
                return page;
            }
            var newPage = new TexturePage(new(128, 128), new(1, 1), 1.1f);
            _pages.Add(characterSize, newPage);
            return newPage;
        }

        private TexturePage? GetPage(uint characterSize)
        {
            _pages.TryGetValue(characterSize, out TexturePage? page);
            return page;
        }

        public Texture? GetTexure(uint characterSize)
        {
            return GetPage(characterSize)?.GetTexture();
        }

        public float GetWhitespace(uint characterSize)
        {
            return GetOrLoadGlyph(' ', characterSize).Advance;
        }

        private static byte[] TranslateBitmap(FTBitmap bitmap)
        {
            var bytes = new byte[bitmap.Width * bitmap.Rows * 4];
            for (int y = 0; y < bitmap.Rows; ++y)
            {
                for (int x = 0; x < bitmap.Width; ++x)
                {
                    int bufferIndex = x + y * bitmap.Pitch;
                    int bitmapIndex = 4 * (x + y * bitmap.Width);
                    byte alpha = 0;
                    if (bitmap.PixelMode == PixelMode.Mono)
                    {
                        if (((bitmap.BufferData[bufferIndex / 8]) & (1 << (7 - (bufferIndex % 8)))) != 0)
                        {
                            alpha = 0b11111111;
                        }
                    }
                    else
                    {
                        alpha = bitmap.BufferData[bufferIndex];
                    }
                    bytes[bitmapIndex] = 255;
                    bytes[bitmapIndex + 1] = 255;
                    bytes[bitmapIndex + 2] = 255;
                    bytes[bitmapIndex + 3] = alpha;
                }
            }
            return bytes;
        }
    }
}
