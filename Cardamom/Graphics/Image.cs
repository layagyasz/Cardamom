using OpenTK.Mathematics;
using StbImageSharp;
using StbImageWriteSharp;

namespace Cardamom.Graphics
{
    public class Image
    {
        public Vector2i Size { get; }

        private readonly Color4[,] _pixels;

        public Image(Vector2i size)
        {
            Size = size;
            _pixels = new Color4[size.X, size.Y];
        }

        public Image(int width, int height)
            : this(new(width, height)) { }

        public static Image FromFile(string path)
        {
            using Stream stream = File.OpenRead(path);
            ImageResult image = ImageResult.FromStream(stream, StbImageSharp.ColorComponents.RedGreenBlueAlpha);
            return FromData(new(image.Width, image.Height), image.Data);
        }

        public static Image FromData(Vector2i size, byte[] data, bool invertYAxis = false)
        {
            var image = new Image(size);
            for (int y = 0; y < size.Y; ++y)
            {
                for (int x = 0; x < size.X; ++x)
                {
                    int index = 4 * (size.X * (invertYAxis ? size.Y - y - 1 : y) + x);
                    image._pixels[x, y] = ToColor(data[index], data[index + 1], data[index + 2], data[index + 3]);
                }
            }
            return image;
        }


        public static Image FromData(Vector2i size, Color4[,] data, bool invertYAxis = false)
        {
            var image = new Image(size);
            for (int y=0; y<size.Y; ++y)
            {
                for (int x=0;x<size.X;++x)
                {
                    var color = data[invertYAxis ? size.Y - y - 1 : y, x];
                    image._pixels[x, y] = 
                        new(
                            MathHelper.Clamp(color.R, 0, 1), 
                            MathHelper.Clamp(color.G, 0, 1), 
                            MathHelper.Clamp(color.B, 0, 1),
                            MathHelper.Clamp(color.A, 0, 1));
                }
            }
            return image;
        }

        public byte[] GetData()
        {
            var data = new byte[4 * Size.X * Size.Y];
            for (int y=0; y<Size.Y;++y)
            {
                for (int x = 0; x < Size.X; ++x)
                {
                    int index = 4 * (Size.X * y + x);
                    Color4 color = Get(x, y);
                    data[index] = ToByte(color.R);
                    data[index + 1] = ToByte(color.G);
                    data[index + 2] = ToByte(color.B);
                    data[index + 3] = ToByte(color.A);
                }
            }
            return data;
        }

        public Color4 Get(int x, int y)
        {
            return _pixels[x, y];
        }

        public void Set(int x, int y, Color4 color)
        {
            _pixels[x, y] = color;
        }

        public void SaveToFile(string path, int jpgQuality = 80)
        {
            using Stream stream = File.OpenWrite(path);
            ImageWriter writer = new();
            string extension = new FileInfo(path).Extension;
            switch (extension)
            {
                case ".bmp":
                    writer.WriteBmp(
                        GetData(), Size.X, Size.Y, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, stream);
                    break;
                case ".png":
                    writer.WritePng(
                        GetData(), Size.X, Size.Y, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, stream);
                    break;
                case ".jpg":
                case ".jpeg":
                    writer.WriteJpg(
                        GetData(),
                        Size.X, 
                        Size.Y,
                        StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, 
                        stream, 
                        jpgQuality);
                    break;
                default:
                    throw new ArgumentException(string.Format($"Unsupported image format [{extension}]"));
            }
        }

        private static Color4 ToColor(byte r, byte g, byte b, byte a)
        {
            return new Color4(r / 255f, g / 255f, b / 255f, a / 255f);
        }

        private static byte ToByte(float channel)
        {
            return (byte)(255 * channel);
        }
    }
}
