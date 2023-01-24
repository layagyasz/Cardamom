using Cardamom.Graphics.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SharpFont;
using StbImageSharp;

namespace Cardamom.Graphics
{
    public class Texture : GLObject
    {
        public Vector2i Size;

        private Texture(int Handle, Vector2i size)
            : base(Handle) 
        {
            Size = size;
        }

        public static Texture Create(Vector2i size)
        {
            int handle = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba32f,
                size.X,
                size.Y,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                0);
            SetParameters();

            return new Texture(handle, size);
        }

        public static Texture Create(Vector2i size, Color4 fill)
        {
            var texture = Create(size);
            texture.Fill(fill);
            return texture;
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
                    PixelInternalFormat.Rgba32f,
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

        public void Bind(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public static void Unbind(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void BindImage(int unit)
        {
            GL.BindImageTexture(unit, Handle, 0, false, 0, TextureAccess.ReadWrite, SizedInternalFormat.Rgba32f);
        }

        public static void UnbindImage(int unit)
        {
            GL.BindImageTexture(unit, 0, 0, false, 0, TextureAccess.ReadWrite, SizedInternalFormat.Rgba32f);
        }

        public Texture Copy()
        {
            var texture = Texture.Create(Size);
            texture.Update(this);
            return texture;
        }

        public Image CopyToImage()
        {
            return Image.FromData(Size, GetData(), /* invertYAxis= */ true);
        }

        public void Fill(Color4 color)
        {
            GL.ClearTexImage(Handle, 0, PixelFormat.Rgba, PixelType.Float, ref color);
        }

        public Color4[,] GetData()
        {
            Bind(TextureUnit.Texture0);
            var data = new Color4[Size.X, Size.Y];
            GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.Float, data);
            return data;
        }

        public void Update(Texture other)
        {
            other.Bind(TextureUnit.Texture0);
            GL.CopyTextureSubImage2D(Handle, 0, 0, 0, 0, 0, other.Size.X, other.Size.Y);
        }

        public void Update(Vector2i offset, Texture other)
        {
            other.Bind(TextureUnit.Texture0);
            GL.CopyTextureSubImage2D(Handle, 0, offset.X, offset.Y, 0, 0, other.Size.X, other.Size.Y);
        }

        public void Update(Vector2i offset, Bitmap bitmap)
        {
            Bind(TextureUnit.Texture0);
            GL.TexSubImage2D(
                TextureTarget.Texture2D, 
                0,
                offset.X,
                offset.Y,
                bitmap.Size.X,
                bitmap.Size.Y, 
                PixelFormat.Rgba,
                PixelType.UnsignedByte, 
                bitmap.Bytes);
        }

        public void Update(Vector2i offset, Color4[,] data)
        {
            Bind(TextureUnit.Texture0);
            GL.TexSubImage2D(
                TextureTarget.Texture2D,
                0,
                offset.X,
                offset.Y,
                data.GetLength(0),
                data.GetLength(1),
                PixelFormat.Rgba,
                PixelType.Float,
                data);
        }

        protected override void DisposeImpl()
        {
            GL.DeleteTexture(Handle);
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
    }
}
