using Cardamom.Mathematics.Geometry;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements.Components
{
    public class RectangleComponent : GraphicsResource
    {
        private static readonly string[] s_Uniforms = 
            { "mode", "size", "border_width", "border_color", "corner_radius" };

        private RenderShader? _shader;
        private UniformBuffer? _uniformBuffer;
        private int[]? _offsets;

        private float[]? _borderWidth;
        private Color4[]? _borderColor;
        private Vector2[]? _cornerRadius;
        private Texture? _texture;

        private readonly VertexArray _vertices = new(PrimitiveType.Triangles, 6);

        public Vector2 Size => (_vertices[5].Position - _vertices[0].Position).Xy;

        public float? GetRayIntersection(Ray3 ray)
        {
            if (ray.Point.X >= _vertices[0].Position.X
                && ray.Point.Y >= _vertices[0].Position.Y
                && ray.Point.X <= _vertices[5].Position.X
                && ray.Point.Y <= _vertices[5].Position.Y)
            {
                return ray.Point.Z / ray.Direction.Z;
            }
            return null;
        }

        public void SetAttributes(ClassAttributes attributes)
        {
            _shader = attributes.BackgroundShader!.Element!;
            _uniformBuffer ??= new(_shader.GetUniformBlockSize("settings"));
            _offsets ??= _shader.GetUniformOffsets(s_Uniforms);

            _borderWidth = attributes.BorderWidth;
            _borderColor = attributes.BorderColor;
            _cornerRadius = attributes.CornerRadius;
            _texture = attributes.Texture.Texture;

            _uniformBuffer.Set(_offsets[0], sizeof(int), _texture == null ? 0 : 1);
            _uniformBuffer.Set(_offsets[1], 2 * sizeof(float), attributes.Size);
            _uniformBuffer.SetArray(_offsets[2], sizeof(float), _borderWidth);
            _uniformBuffer.SetArray(_offsets[3], 4 * sizeof(float), _borderColor);
            _uniformBuffer.SetArray(_offsets[4], 2 * sizeof(float), _cornerRadius);

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
            _uniformBuffer!.Bind(0);
            target.Draw(_vertices, 0, _vertices.Length, _shader!, _texture);
        }

        protected override void DisposeImpl()
        {
            _uniformBuffer?.Dispose();
        }
    }
}
