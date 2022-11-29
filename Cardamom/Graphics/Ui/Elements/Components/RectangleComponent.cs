using Cardamom.Planar;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements.Components
{
    internal class RectangleComponent
    {
        private Shader? _shader;
        private float[]? _borderWidth;
        private Color4[]? _borderColor;
        private Vector2[]? _cornerRadius;

        private readonly VertexArray _vertices = new(PrimitiveType.Triangles, 6);

        public Vector2 Size => _vertices[5].Position - _vertices[0].Position;

        public bool IsPointWithinBounds(Vector2 point)
        {
            return point.X >= _vertices[0].Position.X
                && point.Y >= _vertices[0].Position.Y
                && point.X <= _vertices[5].Position.X
                && point.Y <= _vertices [5].Position.Y;
        }

        public void SetAttributes(ClassAttributes attributes)
        {
            _shader = attributes.Shader!.Element!;
            _borderWidth = attributes.BorderWidth;
            _borderColor = attributes.BorderColor;
            _cornerRadius = attributes.CornerRadius;

            Vector2[] inner =
            {
                new(),
                new(attributes.Size.X, 0),
                attributes.Size,
                new(0, attributes.Size.Y)
            };
            _vertices[0] = new Vertex2(inner[0], attributes.BackgroundColor[0], new());
            _vertices[1] = new Vertex2(inner[1], attributes.BackgroundColor[1], new(1, 0));
            _vertices[2] = new Vertex2(inner[3], attributes.BackgroundColor[3], new(0, 1));
            _vertices[3] = new Vertex2(inner[1], attributes.BackgroundColor[1], new(1, 0));
            _vertices[4] = new Vertex2(inner[3], attributes.BackgroundColor[3], new(0, 1));
            _vertices[5] = new Vertex2(inner[2], attributes.BackgroundColor[2], new(1, 1));
        }

        public void Draw(RenderTarget target, Transform2 transform)
        {
            _shader!.SetVector2("size", Size);
            for (int i=0; i<4; ++i)
            {
                _shader!.SetFloat($"border_width[{i}]", _borderWidth![i]);
                _shader!.SetColor($"border_color[{i}]", _borderColor![i]);
                _shader!.SetVector2($"corner_radius[{i}]", _cornerRadius![i]);
            }
            target.Draw(_vertices, 0, _vertices.Length, transform, _shader!);
        }
    }
}
