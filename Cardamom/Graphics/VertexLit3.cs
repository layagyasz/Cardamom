using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace Cardamom.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexLit3
    {
        [VertexAttribute(0, 3, VertexAttribPointerType.Float, false)]
        public Vector3 Position;

        [VertexAttribute(1, 4, VertexAttribPointerType.Float, false)]
        public Color4 Color;

        [VertexAttribute(2, 2, VertexAttribPointerType.Float, false)]
        public Vector2 TexCoords;

        [VertexAttribute(3, 3, VertexAttribPointerType.Float, true)]
        public Vector3 Normal;

        [VertexAttribute(4, 2, VertexAttribPointerType.Float, false)]
        public Vector2 NormalTexCoords;

        [VertexAttribute(5, 2, VertexAttribPointerType.Float, false)]
        public Vector2 LightingTexCoords;

        public VertexLit3(
            Vector3 position, 
            Color4 color, 
            Vector2 texCoords, 
            Vector3 normal, 
            Vector2 normalTexCoords, 
            Vector2 lighintgTexCoords)
        {
            Position = position;
            Color = color;
            TexCoords = texCoords;
            Normal = normal;
            NormalTexCoords = normalTexCoords;
            LightingTexCoords = lighintgTexCoords;
        }
    }
}
