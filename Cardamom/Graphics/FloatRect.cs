using OpenTK.Mathematics;

namespace Cardamom.Graphics
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

        public override string ToString()
        {
            return string.Format($"[FloatRect: TopLeft={TopLeft}, Size={Size}]");
        }
    }
}
