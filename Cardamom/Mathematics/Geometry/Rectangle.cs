using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Geometry
{
    public class Rectangle : IPolygon
    {
        private readonly Vector2 _topLeft;

        public Vector2 Size { get; }
        public int VertexCount => 4;

        public Rectangle(Vector2 topLeft, Vector2 size)
        {
            _topLeft = topLeft;
            Size = size;
        }

        public bool ContainsPoint(Vector2 point)
        {
            return point.X >= _topLeft.X
                && point.X <= _topLeft.X + Size.X
                && point.Y >= _topLeft.Y
                && point.Y <= _topLeft.Y + Size.Y;
        }

        public float GetArea()
        {
            return Size.X * Size.Y;
        }

        public Vector2 GetVertex(int index)
        {
            return index switch
            {
                1 => _topLeft,
                2 => new(_topLeft.X + Size.X, _topLeft.Y),
                3 => _topLeft + Size,
                4 => new(_topLeft.X, _topLeft.Y + Size.Y),
                _ => throw new IndexOutOfRangeException(),
            };
        }

        public Rectangle GetBounds()
        {
            return this;
        }
    }
}
