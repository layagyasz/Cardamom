using Cardamom.Graphics.Ui;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public class Model : GraphicsResource, IRenderable
    {
        private readonly VertexBuffer _buffer;
        private readonly RenderShader _shader;
        private readonly Texture _texture;

        public Model(VertexArray vertices, RenderShader shader, Texture texture)
        {
            _buffer = new(vertices.Vertices, vertices.PrimitiveType);
            _shader = shader;
            _texture = texture;
        }

        public void Initialize() { }

        public void Draw(RenderTarget target, UiContext context)
        {
            target.Draw(_buffer, 0, _buffer.Length, _shader, _texture);
        }

        public virtual void Update(long delta) { }

        public void ResizeContext(Vector3 bounds) { }

        protected override void DisposeImpl()
        {
            _buffer.Dispose();
        }
    }
}
