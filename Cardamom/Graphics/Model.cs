using Cardamom.Graphics.Ui;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public class Model : IRenderable
    {
        private readonly VertexArray _vertices;
        private readonly Shader _shader;

        public Model(VertexArray vertices, Shader shader)
        {
            _vertices = vertices;
            _shader = shader;
        }

        public void Initialize() { }

        public void Draw(RenderTarget target)
        {
            target.Draw(_vertices, 0, _vertices.Length, _shader, /* texture= */ null);
        }

        public void Update(UiContext context, long delta) { }
    }
}
