using Cardamom.Graphics.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace Cardamom.Graphics
{
    public class Texture : GLObject
    {
        public struct Parameters
        {
            public TextureMinFilter MinFilter { get; set; } = TextureMinFilter.Linear;
            public TextureMagFilter MagFilter { get; set; } = TextureMagFilter.Linear;
            public TextureWrapMode WrapMode { get; set; } = TextureWrapMode.Repeat;

            public Parameters() { }
        }

        public Vector2i Size;

        private Texture(int Handle, Vector2i size)
            : base(Handle) 
        {
            Size = size;
        }

        public static Texture Create(Vector2i size)
        {
            return Create(size, new());
        }

        public static Texture Create(Vector2i size, Parameters parameters)
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
            SetParameters(parameters);

            return new Texture(handle, size);
        }

        public static Texture Create(Vector2i size, Color4 fill, Parameters parameters)
        {
            var texture = Create(size, parameters);
            texture.Fill(fill);
            return texture;
        }

        public static Texture FromFile(string path)
        {
            return FromFile(path, new());
        }

        public static Texture FromFile(string path, Parameters parameters)
        {
            int handle = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);

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
            SetParameters(parameters);
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
            var texture = Create(Size);
            texture.Update(this);
            return texture;
        }

        public Image CopyToImage()
        {
            return Image.FromData(Size, GetData(), /* invertYAxis= */ false);
        }

        public void Fill(Color4 color)
        {
            GL.ClearTexImage(Handle, 0, PixelFormat.Rgba, PixelType.Float, ref color);
        }

        public Color4[,] GetData()
        {
            Bind(TextureUnit.Texture0);
            var data = new Color4[Size.Y, Size.X];
            GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.Float, data);
            return data;
        }

        public void Update(Texture other)
        {
            Update(new(), other);
        }

        public void Update(Vector2i offset, Texture other)
        {
            var buffer = new GLFrameBuffer();
            buffer.Attach(FramebufferAttachment.ColorAttachment0, other);
            GL.CopyTextureSubImage2D(Handle, 0, offset.X, offset.Y, 0, 0, other.Size.X, other.Size.Y);
            buffer.Unbind();
            buffer.Dispose();
            GL.Flush();
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

        private static void SetParameters(Parameters parameters)
        {
            GL.TexParameter(
                TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)parameters.MinFilter);
            GL.TexParameter(
                TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)parameters.MagFilter);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)parameters.WrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)parameters.WrapMode);
        }
    }
}
