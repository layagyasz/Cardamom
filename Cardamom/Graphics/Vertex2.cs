using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public struct Vertex2
    {
        public Vector2 Position { get; set; }
        public Color4 Color { get; set; }
        public Vector2 TexCoords { get; set; }

        public Vertex2(Vector2 position, Color4 color, Vector2 texCoords)
        {
            Position = position;
            Color = color;
            TexCoords = texCoords;
        }
    }
}
