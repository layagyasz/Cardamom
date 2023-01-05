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

        [VertexAttribute(3, 4, VertexAttribPointerType.Float, true)]
        public Quaternion Surface;

        [VertexAttribute(4, 2, VertexAttribPointerType.Float, false)]
        public Vector2 BumpTexCoords;

        public VertexLit3(Vector3 position, Color4 color, Vector2 texCoords, Quaternion surface, Vector2 bumpTexCoords)
        {
            Position = position;
            Color = color;
            TexCoords = texCoords;
            Surface = surface;
            BumpTexCoords = bumpTexCoords;
        }
    }
}
