using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace Cardamom.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex3
    {
        [VertexAttribute(0, 3, VertexAttribPointerType.Float, false)]
        public Vector3 Position;

        [VertexAttribute(1, 4, VertexAttribPointerType.Float, false)]
        public Color4 Color;

        [VertexAttribute(2, 2, VertexAttribPointerType.Float, false)]
        public Vector2 TexCoords;

        public Vertex3(Vector3 position, Color4 color, Vector2 texCoords)
        {
            Position = position;
            Color = color;
            TexCoords = texCoords;
        }
    }
}
