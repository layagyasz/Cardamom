using OpenTK.Graphics.OpenGL4;

namespace Cardamom.Graphics.Core
{
    public class GLBuffer : GLObject
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
        }

        public void Bind()
        {
            GL.BindBuffer(Target, Handle);
            Error.LogGLError("bind buffer");
        }

        public void Bind(int index)
        {
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, index, Handle);
            Error.LogGLError("bind buffer index");
        }

        public void Unbind()
        {
            GL.BindBuffer(Target, 0);
            Error.LogGLError("unbind buffer");
        }

        public static void Unbind(int index)
        {
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, index, 0);
            Error.LogGLError("unbind buffer index");
        }

        public void Allocate(int size)
        {
            Bind();
            GL.BufferData(Target, size, 0, UsageHint);
            Error.LogGLError("allocate");
        }

        public void Buffer<T>(int size, T[] data) where T : struct
        {
            Bind();
            GL.BufferData(Target, size * data.Length, data, UsageHint);
            Error.LogGLError("buffer data");
        }

        public T Read<T>(int offset, int size) where T : struct
        {
            Bind();
            T result = new();
            GL.GetBufferSubData(Target, offset, size, ref result);
            return result;
        }

        public T[] ReadArray<T>(int offset, int size, int length) where T : struct
        {
            Bind();
            T[] result = new T[length];
            GL.GetBufferSubData(Target, offset, size * length, result);
            return result;
        }

        public void SubData<T>(int offset, int size, T data) where T : struct
        {
            Bind();
            GL.BufferSubData(Target, offset, size, ref data);
            Error.LogGLError("sub data");
        }

        public void SubDataArray<T>(int offset, int size, T[] data) where T : struct
        {
            Bind();
            GL.BufferSubData(Target, offset, size * data.Length, data);
            Error.LogGLError("sub data");
        }

        protected override void DisposeImpl()
        {
            GL.DeleteBuffer(Handle);
        }
    }
}
