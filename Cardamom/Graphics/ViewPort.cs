using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public struct ViewPort
    {
        public float Left { get; set; }
        public float Right { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }

        public ViewPort(Vector2i size)
        {
            Left = 0;
            Right = size.X;
            Top = 0;
            Bottom = size.Y;
        }
    }
}
