using Cardamom.Graphics.Core;
using OpenTK.Graphics.OpenGL4;

namespace Cardamom.Graphics
{
    public class VertexBuffer<T> : GraphicsResource where T : struct
    {
        public int Length { get; private set; }
        public PrimitiveType PrimitiveType { get;  set; }

        private readonly GLVertexArray<T> _vertices = new(new());

        public VertexBuffer(PrimitiveType primitiveType)
        {
            PrimitiveType = primitiveType;
        }

        public VertexBuffer(Vertex3[] vertices, PrimitiveType primitiveType)
        {
            _vertices.SetData(vertices);
            PrimitiveType = primitiveType;
            Length = vertices.Length;
        }

        public void Buffer(Vertex3[] vertices)
        {
            Buffer(vertices, 0, vertices.Length);
        }

        public void Buffer(Vertex3[] vertices, int start, int count)
        {
            _vertices.SetData(vertices, start, count);
            Length = count;
        }

        public void Draw(int start, int count)
        {
            _vertices.Draw(PrimitiveType, start, count);
        }

        protected override void DisposeImpl()
        {
            _vertices.Dispose();
        }
    }
}
