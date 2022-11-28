using Cardamom.Graphics.Ui;
using Cardamom.Planar;

namespace Cardamom.Graphics
{
    public interface IRenderable : IInitializable
    {
        void Draw(RenderTarget target, Transform2 transform);
        void Update(UiContext context, Transform2 transform, long delta);
    }
}
