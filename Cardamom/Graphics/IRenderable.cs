using Cardamom.Ui;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public interface IRenderable : IInitializable
    {
        void Draw(RenderTarget target, UiContext context);
        void Update(long delta);
        public void ResizeContext(Vector3 bounds);
    }
}
