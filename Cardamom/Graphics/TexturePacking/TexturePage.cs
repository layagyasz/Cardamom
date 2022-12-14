using OpenTK.Mathematics;

namespace Cardamom.Graphics.TexturePacking
{
    public class TexturePage
    {
        private static readonly float s_MaxRowRatio = 1.4f;

        class Row
        {
            public int Top { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
        }

        public Vector2i Padding { get; }
        public float RowHeightRatio { get; }

        private Color4 _fill;
        private readonly List<Row> _rows = new();
        private Texture _texture;
        private int _nextRowTop;

        public TexturePage(Vector2i initialSize, Color4 fill, Vector2i padding, float rowHeightRatio)
        {
            _fill = fill;
            _texture = Texture.Create(initialSize, fill);
            Padding = padding;
            RowHeightRatio = rowHeightRatio;
        }

        public Texture GetTexture()
        {
            return _texture;
        }

        public Box2i Add(Vector2i size, byte[] bitmap)
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
                if (paddedSize.X + row.Width > _texture.Size.X)
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
            _texture.Update(topLeft, size, bitmap);
            selectedRow.Width += paddedSize.X;
            return rect;
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

            while (_nextRowTop > _texture.Size.Y || width > _texture.Size.X)
            {
                Resize();
            }

            return row;
        }

        private void Resize()
        {
            var newTexture = Texture.Create(2 * _texture.Size, _fill);
            newTexture.Update(_texture);
            _texture.Dispose();
            _texture = newTexture;
        }
    }
}
