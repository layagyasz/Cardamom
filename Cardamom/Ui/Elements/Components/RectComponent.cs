using Cardamom.Graphics;
using Cardamom.Planar;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements.Components
{
    internal class RectComponent
    {
        private readonly VertexArray _vertices = new(PrimitiveType.TriangleFan, 20);

        public Vector2 Size => _vertices[18].Position - _vertices[16].Position;

        public bool IsPointWithinBounds(Vector2 point)
        {
            return point.X >= _vertices[16].Position.X
                && point.Y >= _vertices[16].Position.Y
                && point.X <= _vertices[18].Position.X
                && point.Y <= _vertices [18].Position.Y;
        }

        public void SetAttributes(ClassAttributes attributes)
        {
            Vector2[] inner =
            {
                new(),
                new(attributes.Size.X, 0),
                attributes.Size,
                new(0, attributes.Size.Y)
            };
            Vector2[] outer =
            {
                inner[0] + new Vector2(-attributes.BorderWidth[0], -attributes.BorderWidth[1]),
                inner[1] + new Vector2(attributes.BorderWidth[2], -attributes.BorderWidth[1]),
                inner[2] + new Vector2(attributes.BorderWidth[2], attributes.BorderWidth[3]),
                inner[3] + new Vector2(-attributes.BorderWidth[0], attributes.BorderWidth[3])
            };

            for (uint i = 0; i < 4; ++i)
            {
                _vertices[4 * i] = new Vertex2(outer[i], attributes.BorderColor[i]);
                _vertices[4 * i + 1] = new Vertex2(outer[(i + 1) % 4], attributes.BorderColor[i]);
                _vertices[4 * i + 2] = new Vertex2(inner[(i + 1) % 4], attributes.BorderColor[i]);
                _vertices[4 * i + 3] = new Vertex2(inner[i], attributes.BorderColor[i]);
                _vertices[i + 16] = new Vertex2(inner[i], attributes.BackgroundColor[i]);
            }
        }

        public void Draw(RenderTarget target, Transform2 transform)
        {
            target.Draw(_vertices, 0, _vertices.Length, transform);
        }
    }
}
