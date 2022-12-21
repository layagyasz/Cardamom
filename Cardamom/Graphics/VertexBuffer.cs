using Cardamom.Graphics.Core;
using OpenTK.Graphics.OpenGL4;

namespace Cardamom.Graphics
{
    public class VertexBuffer : GraphicsResource
    {
        public int Length { get; private set; }
        public PrimitiveType PrimitiveType { get;  set; }

        private GLVertexArray _vertices = new(new());

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

        public void SetData(Vertex3[] vertices)
        {
            _vertices.SetData(vertices);
            Length = vertices.Length;
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
