using OpenTK.Graphics.OpenGL4;

namespace Cardamom.Graphics
{
    public class VertexArray
    {
        public PrimitiveType PrimitiveType { get; }
        public int Length => Vertices.Length;
        public Vertex3[] Vertices { get; }

        public Vertex3 this[uint index]
        {
            get => Vertices[index];
            set => Vertices[index] = value;
        }

        public VertexArray(PrimitiveType primitiveType, int size)
        {
            PrimitiveType = primitiveType;
            Vertices = new Vertex3[size];
        }
    }
}
