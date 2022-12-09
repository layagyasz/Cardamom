using OpenTK.Graphics.OpenGL4;

namespace Cardamom.Graphics.Core
{
    public class GLFrameBuffer : GLObject
    {
        public FramebufferTarget Target { get; }

        public GLFrameBuffer(FramebufferTarget target = FramebufferTarget.Framebuffer)
            : base(GL.GenFramebuffer())
        {
            Target = target;
        }

        public void Bind()
        {
            GL.BindFramebuffer(Target, Handle);
        }

        public void Unbind()
        {
            GL.BindFramebuffer(Target, 0);
        }

        public void Attach(FramebufferAttachment attachment, Texture texture)
        {
            Bind();
            GL.FramebufferTexture(Target, attachment, texture.Handle, 0);
        }

        public void Attach(GLRenderBuffer renderBuffer)
        {
            Bind();
            GL.FramebufferRenderbuffer(
                Target, renderBuffer.Attachment, RenderbufferTarget.Renderbuffer, renderBuffer.Handle);
        }

        protected override void DisposeImpl()
        {
            GL.DeleteFramebuffer(Handle);
        }
    }
}
