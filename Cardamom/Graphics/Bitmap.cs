using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public class Bitmap
    {
        public Vector2i Size { get; }
        public byte[] Bytes { get; }

        public Bitmap(Vector2i size, byte[] bytes)
        {
            Size = size;
            Bytes = bytes;
        }
    }
}
