using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Core
{
    public class GLRenderBuffer : GLObject
    {
        public RenderbufferStorage Storage { get; }
        public FramebufferAttachment Attachment { get; }

        public GLRenderBuffer(RenderbufferStorage storage, FramebufferAttachment attachment, Vector2i size)
            : base(GL.GenRenderbuffer()) 
        {
            Storage = storage;
            Attachment = attachment;
            Bind();
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, storage, size.X, size.Y);
        }
        
        public void Bind()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Handle);
        }

        public static void Unbind()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        }

        protected override void DisposeImpl()
        {
            GL.DeleteRenderbuffer(Handle);
        }
    }
}
