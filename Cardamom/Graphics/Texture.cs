using Cardamom.Graphics.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace Cardamom.Graphics
{
    public class Texture : GLObject
    {
        private static readonly Color4 CLEAR = new(1, 1, 1, 0);

        public Vector2i Size;

        private Texture(int Handle, Vector2i size)
            : base(Handle) 
        {
            Size = size;
        }

        public static Texture Create(Vector2i size)
        {
            return Create(size, CLEAR);
        }

        public static Texture Create(Vector2i size, Color4 fill)
        {
            int handle = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                size.X,
                size.Y,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                0);
            GL.ClearTexImage(handle, 0, PixelFormat.Rgba, PixelType.Float, ref fill);
            SetParameters();

            return new Texture(handle, size);
        }

        public static Texture FromFile(string path)
        {
            int handle = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);

            StbImage.stbi_set_flip_vertically_on_load(1);
            Vector2i size;
            using (Stream stream = File.OpenRead(path))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                size = new(image.Width, image.Height);
                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    0, 
                    PixelInternalFormat.Rgba,
                    image.Width,
                    image.Height,
                    0, 
                    PixelFormat.Rgba,
                    PixelType.UnsignedByte,
                    image.Data);
            }
            SetParameters();
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture(handle, size);
        }

        private static void SetParameters()
        {
            GL.TexParameter(
                TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(
                TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }

        public void Bind(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void Update(Texture other)
        {
            Update(new(), other.Size, other.GetBytes());
        }

        public byte[] GetBytes()
        {
            Bind(TextureUnit.Texture0);
            var bytes = new byte[4 * Size.X * Size.Y];
            GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte, bytes);
            return bytes;
        }

        public void Update(Vector2i offset, Vector2i size, byte[] bytes)
        {
            Bind(TextureUnit.Texture0);
            GL.TexSubImage2D(
                TextureTarget.Texture2D, 
                0,
                offset.X,
                offset.Y,
                size.X,
                size.Y, 
                PixelFormat.Rgba,
                PixelType.UnsignedByte, 
                bytes);
        }

        protected override void DisposeImpl()
        {
            GL.DeleteTexture(Handle);
        }
    }
}
