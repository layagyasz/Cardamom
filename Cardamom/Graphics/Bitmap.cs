using OpenTK.Mathematics;
using StbImageSharp;

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

        public static Bitmap FromFile(string path)
        {
            using Stream stream = File.OpenRead(path);
            ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
            return new(new(image.Width, image.Height), image.Data);
        }
    }
}
