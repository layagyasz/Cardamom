using Cardamom.Ui;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public interface IRenderable : IInitializable
    {
        void Draw(RenderTarget target, UiContext context);
        void Update(long delta);
        void ResizeContext(Vector3 bounds);
    }
}
