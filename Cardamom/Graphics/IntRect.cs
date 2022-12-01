using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public struct IntRect
    {
        public Vector2i TopLeft { get; set; }
        public Vector2i Size { get; set; }

        public IntRect(Vector2i topLeft, Vector2i size)
        {
            TopLeft=  topLeft;
            Size = size;
        }
    }
}
