using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Core
{
    public class GLVertexArray : GLObject
    {
        private static readonly int POSITION_ATTRIBUTE_INDEX = 0;
        private static readonly int COLOR_ATTRIBUTE_INDEX = 1;

        private readonly GLBuffer<Vertex2> _buffer;

        public GLVertexArray(GLBuffer<Vertex2> buffer)
            : base(GL.GenVertexArray())
        {
            _buffer = buffer;

            Bind();
            GL.VertexAttribPointer(
                POSITION_ATTRIBUTE_INDEX, 
                2, 
                VertexAttribPointerType.Float, 
                /* normalized= */ false, 
                2 * sizeof(float),
                0);
            GL.EnableVertexAttribArray(POSITION_ATTRIBUTE_INDEX);
            GL.VertexAttribPointer(
                COLOR_ATTRIBUTE_INDEX,
                4, 
                VertexAttribPointerType.Float,
                /* normalized= */ false, 4 * sizeof(float), 
                0);
            GL.EnableVertexAttribArray(COLOR_ATTRIBUTE_INDEX);
        }

        public void Bind()
        {
            GL.BindVertexArray(Handle);
        }

        public void Draw(PrimitiveType primitiveType, int start, int count)
        {
            Bind();
            GL.DrawArrays(primitiveType, start, count);
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
