using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace Cardamom.Graphics.Core
{
    public class GLBuffer<T> : GLObject where T : struct
    {
        public BufferTarget BufferTarget { get; }
        public BufferUsageHint BufferUsageHint { get; }
        public int ElementSize { get; }

        public GLBuffer(
            BufferTarget bufferTarget = BufferTarget.ArrayBuffer,
            BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw)
            : base(GL.GenBuffer())
        {
            BufferTarget = bufferTarget;
            BufferUsageHint = bufferUsageHint;
            ElementSize = Marshal.SizeOf(typeof(T));
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget, Handle);
        }

        public void SetData(T[] data)
        {
            Bind();
            GL.BufferData(BufferTarget, ElementSize * data.Length, data, BufferUsageHint);
        }

        protected override void DisposeImpl()
        {
            GL.DeleteBuffer(Handle);
        }
    }
}
