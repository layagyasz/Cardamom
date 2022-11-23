using Cardamom.Graphics;
using Cardamom.Planar;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements.Components
{
    internal class RectangleComponent
    {
        private readonly VertexArray _vertices = new(PrimitiveType.Triangles, 30);

        public Vector2 Size => _vertices[29].Position - _vertices[24].Position;

        public bool IsPointWithinBounds(Vector2 point)
        {
            return point.X >= _vertices[24].Position.X
                && point.Y >= _vertices[24].Position.Y
                && point.X <= _vertices[29].Position.X
                && point.Y <= _vertices [29].Position.Y;
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
                _vertices[6 * i] = new Vertex2(outer[i], attributes.BorderColor[i]);
                _vertices[6 * i + 1] = new Vertex2(outer[(i + 1) % 4], attributes.BorderColor[i]);
                _vertices[6 * i + 2] = new Vertex2(inner[(i + 1) % 4], attributes.BorderColor[i]);
                _vertices[6 * i + 3] = _vertices[6 * i];
                _vertices[6 * i + 4] = _vertices[6 * i + 2];
                _vertices[6 * i + 5] = new Vertex2(inner[i], attributes.BorderColor[i]);
            }

            _vertices[24] = new Vertex2(inner[0], attributes.BackgroundColor[0]);
            _vertices[25] = new Vertex2(inner[1], attributes.BackgroundColor[1]);
            _vertices[26] = new Vertex2(inner[3], attributes.BackgroundColor[3]);
            _vertices[27] = new Vertex2(inner[1], attributes.BackgroundColor[1]);
            _vertices[28] = new Vertex2(inner[3], attributes.BackgroundColor[3]);
            _vertices[29] = new Vertex2(inner[2], attributes.BackgroundColor[2]);
        }

        public void Draw(RenderTarget target, Transform2 transform)
        {
            target.Draw(_vertices, 0, _vertices.Length, transform);
        }
    }
}
