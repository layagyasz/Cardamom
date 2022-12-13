using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public struct Vertex3
    {
        public Vector3 Position { get; set; }
        public Color4 Color { get; set; }
        public Vector2 TexCoords { get; set; }

        public Vertex3(Vector3 position, Color4 color, Vector2 texCoords)
        {
            Position = position;
            Color = color;
            TexCoords = texCoords;
        }
    }
}
