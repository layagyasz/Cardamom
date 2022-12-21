using Cardamom.Graphics.Ui;

namespace Cardamom.Graphics
{
    public class Model : GraphicsResource, IRenderable
    {
        private readonly VertexBuffer _buffer;
        private readonly Shader _shader;
        private readonly Texture _texture;

        public Model(VertexArray vertices, Shader shader, Texture texture)
        {
            _buffer = new(vertices.Vertices, vertices.PrimitiveType);
            _shader = shader;
            _texture = texture;
        }

        public void Initialize() { }

        public void Draw(RenderTarget target)
        {
            target.Draw(_buffer, 0, _buffer.Length, _shader, _texture);
        }

        public void Update(UiContext context, long delta) { }

        protected override void DisposeImpl()
        {
            _buffer.Dispose();
        }
    }
}
