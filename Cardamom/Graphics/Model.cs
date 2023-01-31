using Cardamom.Ui;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public class Model<T> : IRenderable where T : struct
    {
        private readonly VertexBuffer<T> _buffer;
        private readonly RenderShader _shader;
        private readonly Material _material;

        public Model(VertexBuffer<T> buffer, RenderShader shader, Material material)
        {
            _buffer = buffer;
            _shader = shader;
            _material = material;
        }

        public void Initialize() { }

        public void Draw(RenderTarget target, UiContext context)
        {
            target.Draw(
                _buffer,
                0, 
                _buffer.Length, 
                new(BlendMode.None, _shader, _material.Diffuse, _material.Normal, _material.Lighting));
        }

        public virtual void Update(long delta) { }

        public void ResizeContext(Vector3 bounds) { }
    }
}
