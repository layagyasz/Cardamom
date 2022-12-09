using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public struct ViewPort
    {
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }

        public ViewPort(Vector2i size)
        {
            Left = 0;
            Right = size.X;
            Top = 0;
            Bottom = size.Y;
        }
    }
}
