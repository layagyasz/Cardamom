using Cardamom.Graphics;
using Cardamom.Mathematics.Geometry;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements.Components
{
    public class RectangleComponent : ManagedResource
    {
        private RenderShader? _shader;

        private bool _disableDraw;
        private Vector2 _size;
        private Texture? _texture;
        private UniformBuffer? _uniforms;

        private readonly Vertex3[] _vertices = new Vertex3[6];

        public Vector2 Size => (_vertices[5].Position - _vertices[0].Position).Xy;

        protected override void DisposeImpl() { }

        public float? GetRayIntersection(Ray3 ray)
        {
            if (ray.Point.X >= 0 && ray.Point.Y >= 0 && ray.Point.X <= _size.X && ray.Point.Y <= _size.Y)
            {
                return ray.Point.Z / ray.Direction.Z;
            }
            return null;
        }

        public void SetAttributes(ClassAttributes attributes)
        {
            _disableDraw = attributes.DisableDraw;
            _shader = attributes.BackgroundShader;
            _texture = attributes.Texture.Texture;
            _uniforms = attributes.GetUniforms();

            Vector2 topLeft = attributes.Texture.TextureView.Min;
            Vector2 bottomRight = attributes.Texture.TextureView.Max;
            _vertices[0] = new(new(), attributes.BackgroundColor[0], topLeft);
            _vertices[1] = new(_vertices[1].Position, attributes.BackgroundColor[1], new(bottomRight.X, topLeft.Y));
            _vertices[2] = new(_vertices[2].Position, attributes.BackgroundColor[3], new(topLeft.X, bottomRight.Y));
            _vertices[3] = new(_vertices[3].Position, attributes.BackgroundColor[1], new(bottomRight.X, topLeft.Y));
            _vertices[4] = new(_vertices[4].Position, attributes.BackgroundColor[3], new(topLeft.X, bottomRight.Y));
            _vertices[5] = new(_vertices[5].Position, attributes.BackgroundColor[2], bottomRight);
        }

        public void SetSize(Vector2 size)
        {
            _size = size;
            _vertices[1].Position = new(size.X, 0, 0);
            _vertices[2].Position = new(0, size.Y, 0);
            _vertices[3].Position = new(size.X, 0, 0);
            _vertices[4].Position = new(0, size.Y, 0);
            _vertices[5].Position = new(size.X, size.Y, 0);
        }

        public void Draw(IRenderTarget target)
        {
            if (!_disableDraw)
            {
                _uniforms!.Bind(0);
                _shader!.SetVector2("size", _size);
                target.Draw(
                    _vertices,
                    PrimitiveType.Triangles,
                    0,
                    _vertices.Length,
                    _texture == null ? new(BlendMode.Alpha, _shader!) : new(BlendMode.Alpha, _shader!, _texture!));
            }
        }
    }
}
