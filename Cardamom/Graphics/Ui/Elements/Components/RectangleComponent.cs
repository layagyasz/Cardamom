using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements.Components
{
    public class RectangleComponent
    {
        private Shader? _shader;
        private float[]? _borderWidth;
        private Color4[]? _borderColor;
        private Vector2[]? _cornerRadius;
        private Texture? _texture;

        private readonly VertexArray _vertices = new(PrimitiveType.Triangles, 6);

        public Vector2 Size => (_vertices[5].Position - _vertices[0].Position).Xy;

        public bool IntersectsRay(Vector3 origin, Vector3 direction)
        {
            return origin.X >= _vertices[0].Position.X
                && origin.Y >= _vertices[0].Position.Y
                && origin.X <= _vertices[5].Position.X
                && origin.Y <= _vertices [5].Position.Y;
        }

        public void SetAttributes(ClassAttributes attributes)
        {
            _shader = attributes.BackgroundShader!.Element!;
            _borderWidth = attributes.BorderWidth;
            _borderColor = attributes.BorderColor;
            _cornerRadius = attributes.CornerRadius;
            _texture = attributes.Texture.Texture;

            Vector2 topLeft = attributes.Texture.TextureView.Min;
            Vector2 bottomRight = attributes.Texture.TextureView.Max;
            _vertices[0] = new Vertex3(new(), attributes.BackgroundColor[0], topLeft);
            _vertices[1] = 
                new Vertex3(
                    new(attributes.Size.X, 0, 0), attributes.BackgroundColor[1], new(bottomRight.X, topLeft.Y));
            _vertices[2] = 
                new Vertex3(
                    new(0, attributes.Size.Y, 0), attributes.BackgroundColor[3], new(topLeft.X, bottomRight.Y));
            _vertices[3] = 
                new Vertex3(
                    new(attributes.Size.X, 0, 0), attributes.BackgroundColor[1], new(bottomRight.X, topLeft.Y));
            _vertices[4] = 
                new Vertex3(
                    new(0, attributes.Size.Y, 0), attributes.BackgroundColor[3], new(topLeft.X, bottomRight.Y));
            _vertices[5] = 
                new Vertex3(new(attributes.Size.X, attributes.Size.Y, 0), attributes.BackgroundColor[2], bottomRight);
        }

        public void Draw(RenderTarget target)
        {
            _shader!.SetInt32("mode", _texture == null ? 0 : 1);
            _shader!.SetVector2("size", Size);
            for (int i=0; i<4; ++i)
            {
                _shader!.SetFloat($"border_width[{i}]", _borderWidth![i]);
                _shader!.SetColor($"border_color[{i}]", _borderColor![i]);
                _shader!.SetVector2($"corner_radius[{i}]", _cornerRadius![i]);
            }
            target.Draw(_vertices, 0, _vertices.Length, _shader!, _texture);
        }
    }
}
