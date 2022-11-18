using SFML.Graphics;
using SFML.System;

namespace Cardamom.Ui.Elements.Components
{
    internal class RectComponent
    {
        private readonly VertexArray _vertices = new(PrimitiveType.Quads, 20);

        public Vector2f Size => _vertices[18].Position - _vertices[16].Position;

        public bool IsPointWithinBounds(Vector2f point)
        {
            return point.X >= _vertices[16].Position.X
                && point.Y >= _vertices[16].Position.Y
                && point.X <= _vertices[18].Position.X
                && point.Y <= _vertices [18].Position.Y;
        }

        public void SetAttributes(ClassAttributes attributes)
        {
            Vector2f[] inner =
            {
                new Vector2f(0, 0),
                new Vector2f(attributes.Size.X, 0),
                attributes.Size,
                new Vector2f(0, attributes.Size.Y)
            };
            Vector2f[] outer =
            {
                inner[0] + new Vector2f(-attributes.BorderWidth[0], -attributes.BorderWidth[1]),
                inner[1] + new Vector2f(attributes.BorderWidth[2], -attributes.BorderWidth[1]),
                inner[2] + new Vector2f(attributes.BorderWidth[2], attributes.BorderWidth[3]),
                inner[3] + new Vector2f(-attributes.BorderWidth[0], attributes.BorderWidth[3])
            };

            for (uint i = 0; i < 4; ++i)
            {
                _vertices[4 * i] = new Vertex(outer[i], attributes.BorderColor[i]);
                _vertices[4 * i + 1] = new Vertex(outer[(i + 1) % 4], attributes.BorderColor[i]);
                _vertices[4 * i + 2] = new Vertex(inner[(i + 1) % 4], attributes.BorderColor[i]);
                _vertices[4 * i + 3] = new Vertex(inner[i], attributes.BorderColor[i]);
                _vertices[i + 16] = new Vertex(inner[i], attributes.BackgroundColor[i]);
            }
        }

        public void Draw(RenderTarget target, Transform transform)
        {
            target.Draw(_vertices, new RenderStates(transform));
        }
    }
}
