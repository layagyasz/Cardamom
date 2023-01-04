using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace Cardamom.Graphics.Core
{
    public class GLVertexArray : GLObject
    {
        private static readonly int s_PositionAttributeIndex = 0;
        private static readonly int s_ColorAttributeIndex = 1;
        private static readonly int s_TexCoordsAttributeIndex = 2;

        private readonly int _size;
        private readonly GLBuffer _buffer;

        public GLVertexArray(GLBuffer buffer)
            : base(GL.GenVertexArray())
        {
            _size = Marshal.SizeOf(typeof(Vertex3));
            _buffer = buffer;

            Bind();
            _buffer.Bind();

            GL.VertexAttribPointer(
                s_PositionAttributeIndex, 
                3, 
                VertexAttribPointerType.Float, 
                /* normalized= */ false, 
                9 * sizeof(float),
                0);
            Error.LogGLError("link position attribute");

            GL.EnableVertexAttribArray(s_PositionAttributeIndex);
            Error.LogGLError("enable position attribute");

            GL.VertexAttribPointer(
                s_ColorAttributeIndex,
                4, 
                VertexAttribPointerType.Float,
                /* normalized= */ false, 
                9 * sizeof(float), 
                3 * sizeof(float));
            Error.LogGLError("link color attribute");

            GL.EnableVertexAttribArray(s_ColorAttributeIndex);
            Error.LogGLError("enable color attribute");

            GL.VertexAttribPointer(
                s_TexCoordsAttributeIndex,
                2,
                VertexAttribPointerType.Float,
                /* normalized= */ false,
                9 * sizeof(float),
                7 * sizeof(float));
            Error.LogGLError("link tex coord attribute");

            GL.EnableVertexAttribArray(s_TexCoordsAttributeIndex);
            Error.LogGLError("enable tex coord attribute");
        }

        public void Bind()
        {
            GL.BindVertexArray(Handle);
            Error.LogGLError("bind vertex array");
        }

        public void Draw(PrimitiveType primitiveType, int start, int count)
        {
            Bind();
            GL.DrawArrays(primitiveType, start, count);
            Error.LogGLError($"draw {primitiveType} vertex array");
        }

        public void SetData(Vertex3[] data)
        {
            _buffer.Buffer(_size, data);
        }

        public void SetData(Vertex3[] data, int start, int count)
        {
            _buffer.Buffer(_size, data, start, count);
        }

        protected override void DisposeImpl()
        {
            GL.DeleteVertexArray(Handle);
            _buffer.Dispose();
        }
    }
}
