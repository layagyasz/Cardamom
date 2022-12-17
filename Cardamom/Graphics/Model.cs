using Cardamom.Graphics.Ui;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public class Model : IRenderable
    {
        private readonly VertexArray _vertices;
        private readonly Shader _shader;
        private readonly Texture _texture;

        public Model(VertexArray vertices, Shader shader, Texture texture)
        {
            _vertices = vertices;
            _shader = shader;
            _texture = texture;
        }

        public void Initialize() { }

        public void Draw(RenderTarget target)
        {
            target.Draw(_vertices, 0, _vertices.Length, _shader, _texture);
        }

        public void Update(UiContext context, long delta) { }
    }
}
