using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Core
{
    public class GLVertexArray : GLObject
    {
        private static readonly int POSITION_ATTRIBUTE_INDEX = 0;
        private static readonly int COLOR_ATTRIBUTE_INDEX = 1;
        private static readonly int TEX_COORDS_ATTRIBUTE_INDEX = 2;

        private readonly GLBuffer<Vertex2> _buffer;

        public GLVertexArray(GLBuffer<Vertex2> buffer)
            : base(GL.GenVertexArray())
        {
            _buffer = buffer;

            Bind();
            _buffer.Bind();

            GL.VertexAttribPointer(
                POSITION_ATTRIBUTE_INDEX, 
                2, 
                VertexAttribPointerType.Float, 
                /* normalized= */ false, 
                8 * sizeof(float),
                0);
            Error.LogGLError("link position attribute");

            GL.EnableVertexAttribArray(POSITION_ATTRIBUTE_INDEX);
            Error.LogGLError("enable position attribute");

            GL.VertexAttribPointer(
                COLOR_ATTRIBUTE_INDEX,
                4, 
                VertexAttribPointerType.Float,
                /* normalized= */ false, 
                8 * sizeof(float), 
                2 * sizeof(float));
            Error.LogGLError("link color attribute");

            GL.EnableVertexAttribArray(COLOR_ATTRIBUTE_INDEX);
            Error.LogGLError("enable color attribute");

            GL.VertexAttribPointer(
                TEX_COORDS_ATTRIBUTE_INDEX,
                2,
                VertexAttribPointerType.Float,
                /* normalized= */ false,
                8 * sizeof(float),
                6 * sizeof(float));
            Error.LogGLError("link tex coord attribute");

            GL.EnableVertexAttribArray(TEX_COORDS_ATTRIBUTE_INDEX);
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

        public void SetData(Vertex2[] data)
        {
            _buffer.SetData(data);
        }

        protected override void DisposeImpl()
        {
            GL.DeleteVertexArray(Handle);
            _buffer.Dispose();
        }
    }
}
