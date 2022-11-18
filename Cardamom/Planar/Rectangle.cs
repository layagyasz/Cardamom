using SFML.System;

namespace Cardamom.Planar
{
    public class Rectangle : IPolygon
    {
        private readonly Vector2f _topLeft;

        public Vector2f Size { get; }
        public int VertexCount => 4;

        public Rectangle(Vector2f topLeft, Vector2f size)
        {
            _topLeft = topLeft;
            Size = size;
        }

        public bool ContainsPoint(Vector2f point)
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

        public Vector2f GetVertex(int index)
        {
            return index switch
            {
                1 => _topLeft,
                2 => new Vector2f(_topLeft.X + Size.X, _topLeft.Y),
                3 => _topLeft + Size,
                4 => new Vector2f(_topLeft.X, _topLeft.Y + Size.Y),
                _ => throw new IndexOutOfRangeException(),
            };
        }

        public Rectangle GetBounds()
        {
            return this;
        }
    }
}
