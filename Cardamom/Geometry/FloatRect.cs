using OpenTK.Mathematics;

namespace Cardamom.Geometry
{
    public struct FloatRect
    {
        public Vector2 TopLeft { get; set; }
        public Vector2 Size { get; set; }

        public FloatRect(Vector2 topLeft, Vector2 size)
        {
            TopLeft = topLeft;
            Size = size;
        }

        public bool Contains(Vector2 point)
        {
            return point.X > TopLeft.X
                && point.Y > TopLeft.Y
                && point.X < TopLeft.X + Size.X
                && point.Y < TopLeft.Y + Size.Y;
        }

        public FloatRect GetIntersection(FloatRect other)
        {
            Vector2 topLeft = new(Math.Max(TopLeft.X, other.TopLeft.X), Math.Max(TopLeft.Y, other.TopLeft.Y));
            Vector2 bottomRight =
                new(
                    Math.Min(TopLeft.X + Size.X, other.TopLeft.X + other.Size.X),
                    Math.Min(TopLeft.Y + Size.Y, other.TopLeft.Y + other.Size.Y));
            return new(topLeft, bottomRight - topLeft);
        }

        public override string ToString()
        {
            return string.Format($"[FloatRect: TopLeft={TopLeft}, Size={Size}]");
        }
    }
}
