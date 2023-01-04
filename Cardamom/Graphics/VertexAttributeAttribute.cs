using OpenTK.Graphics.OpenGL4;

namespace Cardamom.Graphics
{
    [AttributeUsage(AttributeTargets.Field)]
    public class VertexAttributeAttribute : Attribute
    {
        public int Index { get; set; }
        public int Cardinality { get; set; }
        public VertexAttribPointerType Type { get; set; }
        public bool Normalized { get; set; }

        public VertexAttributeAttribute(int index, int cardinality, VertexAttribPointerType type, bool normalized)
        {
            Index = index;
            Cardinality = cardinality;
            Type = type;
            Normalized = normalized;
        }
    }
}
