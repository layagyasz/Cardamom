using Cardamom.Ui;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public class Model<T> : IRenderable where T : struct
    {
        public VertexBuffer<T> Buffer { get; }
        public RenderShader Shader { get; }
        public Material Material { get; }

        public Model(VertexBuffer<T> buffer, RenderShader shader, Material material)
        {
            Buffer = buffer;
            Shader = shader;
            Material = material;
        }

        public void Initialize() { }

        public void Draw(RenderTarget target, UiContext context)
        {
            target.Draw(
                Buffer,
                0, 
                Buffer.Length, 
                new(BlendMode.None, Shader, Material.Diffuse, Material.Normal, Material.Lighting));
        }

        public virtual void Update(long delta) { }

        public void ResizeContext(Vector3 bounds) { }
    }
}
