using SFML.System;

namespace Cardamom.Planar
{
    public class Rectangle : IPolygon
    {
        private readonly Vector2f _topLeft;
        private readonly Vector2f _size;

        public int VertexCount => 4;

        public Rectangle(Vector2f topLeft, Vector2f size)
        {
            _topLeft = topLeft;
            _size = size;
        }

        public bool ContainsPoint(Vector2f point)
        {
            return point.X >= _topLeft.X
                && point.X <= _topLeft.X + _size.X 
                && point.Y >= _topLeft.Y 
                && point.Y <= _topLeft.Y + _size.Y;
        }

        public float GetArea()
        {
            return _size.X * _size.Y;
        }

        public Vector2f GetVertex(int index)
        {
            return index switch
            {
                1 => _topLeft,
                2 => new Vector2f(_topLeft.X + _size.X, _topLeft.Y),
                3 => _topLeft + _size,
                4 => new Vector2f(_topLeft.X, _topLeft.Y + _size.Y),
                _ => throw new IndexOutOfRangeException(),
            };
        }

        public Rectangle GetBounds()
        {
            return this;
        }
    }
}
