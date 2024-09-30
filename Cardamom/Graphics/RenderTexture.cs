using Cardamom.Graphics.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public class RenderTexture : BaseRenderTarget, IDisposable
    {
        public Vector2i Size { get; }

        private readonly Texture _texture;
        private readonly GLRenderBuffer _depthBuffer;
        private readonly GLFrameBuffer _frameBuffer;

        public RenderTexture(Vector2i size)
            : base(new(new(), size))
        {
            Size = size;

            _texture = Texture.Create(size, Color4.Black, new());
            _depthBuffer = 
                new GLRenderBuffer(
                    RenderbufferStorage.Depth24Stencil8, FramebufferAttachment.DepthStencilAttachment, size);
            _frameBuffer = new GLFrameBuffer();

            _frameBuffer.Attach(FramebufferAttachment.ColorAttachment0, _texture);
            _frameBuffer.Attach(_depthBuffer);
            _frameBuffer.Unbind();
        }

        public override void SetActive(bool active)
        {
            if(active)
            {
                _frameBuffer.Bind();
                var viewPort = GetViewPort();
                GL.Viewport(viewPort.Min.X, viewPort.Min.Y, viewPort.Size.X, viewPort.Size.Y);
            }
            else
            {
                _frameBuffer.Unbind();
            }
        }

        public override void Display() { }

        public Texture GetTexture()
        {
            return _texture;
        }

        public Image CopyToImage()
        {
            return _texture.CopyToImage();
        }

        public void Dispose()
        {
            _texture.Dispose();
            _depthBuffer.Dispose();
            _frameBuffer.Dispose();
            GC.SuppressFinalize(this);
            GC.KeepAlive(this);
        }
    }
}
