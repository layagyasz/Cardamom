using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace Cardamom.Graphics.Core
{
    public class GLBuffer<T> : GLObject where T : struct
    {
        public BufferTarget Target { get; }
        public BufferUsageHint UsageHint { get; }
        public int ElementSize { get; }

        public GLBuffer(
            BufferTarget target = BufferTarget.ArrayBuffer,
            BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
            : base(GL.GenBuffer())
        {
            Target = target;
            UsageHint = usageHint;
            ElementSize = Marshal.SizeOf(typeof(T));
        }

        public void Bind()
        {
            GL.BindBuffer(Target, Handle);
            Error.LogGLError("bind buffer");
        }

        public void SetData(T[] data)
        {
            Bind();
            GL.BufferData(Target, ElementSize * data.Length, data, UsageHint);
            Error.LogGLError("buffer data");
        }

        protected override void DisposeImpl()
        {
            GL.DeleteBuffer(Handle);
        }
    }
}
