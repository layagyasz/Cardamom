using Cardamom.Mathematics;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.TexturePacking
{
    public class DynamicVariableSizeTexturePage : GraphicsResource, ITexturePage
    {
        private static readonly float s_MaxRowRatio = 1.4f;

        public class Supplier : ITexturePageSupplier
        {
            public IntInterval WidthRange { get; }
            public IntInterval HeightRange { get; }
            public Color4 Fill { get; }
            public Vector2i Padding { get; }
            public float RowHeightRatio { get; }

            public Supplier(
                IntInterval widthRange, IntInterval heightRange, Color4 fill, Vector2i padding, float rowHeightRatio)
            {
                WidthRange = widthRange;
                HeightRange = heightRange;
                Fill = fill;
                Padding = padding;
                RowHeightRatio = rowHeightRatio;
            }

            public ITexturePage Get()
            {
                return new DynamicVariableSizeTexturePage(
                    new(WidthRange.Minimum, HeightRange.Minimum), Fill, Padding, RowHeightRatio);
            }
        }

        class Row
        {
            public int Top { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
        }

        public Vector2i Padding { get; }
        public float RowHeightRatio { get; }

        private readonly Color4 _fill;
        private readonly List<Row> _rows = new();

        private Texture? _texture;
        private int _nextRowTop;

        public DynamicVariableSizeTexturePage(
            Vector2i initialSize, Color4 fill, Vector2i padding, float rowHeightRatio)
        {
            _fill = fill;
            _texture = Texture.Create(initialSize, fill);
            Padding = padding;
            RowHeightRatio = rowHeightRatio;
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
            if (ReserveSpace(texture.Size, out bounds))
            {
                _texture!.Update(bounds.Min, texture);
                return true;
            }
            return false;
        }

        public bool Add(Bitmap bitmap, out Box2i bounds)
        {
            if (ReserveSpace(bitmap.Size, out bounds))
            {
                _texture!.Update(bounds.Min, bitmap);
                return true;
            }
            return false;
        }

        private bool ReserveSpace(Vector2i size, out Box2i bounds)
        {
            Vector2i paddedSize = size + 2 * Padding;
            Row? selectedRow = null;
            float selectedRatio = float.MaxValue;
            foreach (var row in _rows)
            {
                var ratio = (float)row.Height / paddedSize.Y;
                if (ratio > s_MaxRowRatio || ratio < 1)
                {
                    continue;
                }
                if (paddedSize.X + row.Width > _texture!.Size.X)
                {
                    continue;
                }
                if (ratio > selectedRatio)
                {
                    continue;
                }
                selectedRow = row;
                selectedRatio = ratio;
            }

            selectedRow ??= AddRow(paddedSize.X, (int)(RowHeightRatio * paddedSize.Y));
            var topLeft = Padding + new Vector2i(selectedRow.Width, selectedRow.Top);
            var rect = new Box2i(topLeft, topLeft + size);
            selectedRow.Width += paddedSize.X;

            bounds = rect;
            return true;
        }

        private Row AddRow(int width, int height)
        {
            var row =
                new Row()
                {
                    Top = _nextRowTop,
                    Height = height,
                    Width = 0
                };

            _nextRowTop += height;
            _rows.Add(row);

            while (_nextRowTop > _texture!.Size.Y || width > _texture.Size.X)
            {
                Resize();
            }

            return row;
        }

        private void Resize()
        {
            var newTexture = Texture.Create(2 * _texture!.Size, _fill);
            newTexture.Update(_texture);
            _texture.Dispose();
            _texture = newTexture;
        }
    }
}
