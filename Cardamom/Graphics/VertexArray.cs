using OpenTK.Graphics.OpenGL4;

namespace Cardamom.Graphics
{
    public class VertexArray
    {
        public PrimitiveType PrimitiveType { get; }
        public int Length => Vertices.Length;
        public Vertex2[] Vertices { get; }

        public Vertex2 this[uint index]
        {
            get => Vertices[index];
            set => Vertices[index] = value;
        }

        public VertexArray(PrimitiveType primitiveType, int size)
        {
            PrimitiveType = primitiveType;
            Vertices = new Vertex2[size];
        }
    }
}
