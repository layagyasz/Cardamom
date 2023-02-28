using OpenTK.Mathematics;

namespace Cardamom.Graphics.TexturePacking
{
    public class DynamicStaticSizeTexturePage : GraphicsResource, ITexturePage
    {
        public class Supplier : ITexturePageSupplier
        {
            public Vector2i Size { get; }
            public Vector2i ElementSize { get; }
            public Color4 Fill { get; }
            public Vector2i Padding { get; }

            public Supplier(Vector2i size, Vector2i elementSize, Color4 fill, Vector2i padding)
            {
                Size = size;
                ElementSize = elementSize;
                Fill = fill;
                Padding = padding;
            }

            public ITexturePage Get()
            {
                return new DynamicStaticSizeTexturePage(Size, ElementSize, Fill, Padding);
            }
        }

        public Vector2i Size { get; }
        public Vector2i ElementSize { get; }
        public Vector2i Padding { get; }

        private Texture? _texture;

        private Vector2i _cursor;

        public DynamicStaticSizeTexturePage(Vector2i size, Vector2i elementSize, Color4 fill, Vector2i padding)
        {
            Size = size;
            ElementSize = elementSize;
            Padding = padding;
            _texture = Texture.Create(size, fill);
        }

        protected override void DisposeImpl()
        {
            _texture!.Dispose();
            _texture = null;
        }

        public Texture GetTexture()
        {
            return _texture!;
        }

        public bool Add(Texture texture, out Box2i bounds)
        {
            if (ReserveSpace(out bounds))
            {
                _texture!.Update(bounds.Min, texture);
                return true;
            }
            return false;
        }

        public bool Add(Bitmap bitmap, out Box2i bounds)
        {
            if (ReserveSpace(out bounds))
            {
                _texture!.Update(bounds.Min, bitmap);
                return true;
            }
            return false;
        }

        private bool ReserveSpace(out Box2i bounds)
        {
            var paddedSize = ElementSize + 2 * Padding;
            if (Size.X >= _cursor.X + paddedSize.X)
            {
                bounds = new(_cursor + Padding, _cursor + Padding + ElementSize);
                _cursor.X += paddedSize.X;
                return true;
            }
            _cursor.X = 0;
            _cursor.Y += ElementSize.Y;
            if (Size.Y >= _cursor.Y + paddedSize.Y)
            {
                bounds = new(_cursor + Padding, _cursor + Padding + ElementSize);
                _cursor.X += paddedSize.X;
                return true;
            }
            bounds = new();
            return false;
        }
    }
}
