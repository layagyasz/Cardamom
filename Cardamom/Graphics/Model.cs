using Cardamom.Graphics.Ui;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public class Model<T> : GraphicsResource, IRenderable where T : struct
    {
        private readonly VertexBuffer<T> _buffer;
        private readonly RenderShader _shader;
        private readonly Texture _texture;
        private readonly Texture _bumpTexture;

        public Model(
            T[] vertices, PrimitiveType primitiveType, RenderShader shader, Texture texture, Texture bumpTexture)
        {
            _buffer = new(vertices, primitiveType);
            _shader = shader;
            _texture = texture;
            _bumpTexture = bumpTexture;
        }

        public void Initialize() { }

        public void Draw(RenderTarget target, UiContext context)
        {
            target.Draw(_buffer, 0, _buffer.Length, new(BlendMode.None, _shader, _texture, _bumpTexture));
        }

        public virtual void Update(long delta) { }

        public void ResizeContext(Vector3 bounds) { }

        protected override void DisposeImpl()
        {
            _buffer.Dispose();
        }
    }
}
