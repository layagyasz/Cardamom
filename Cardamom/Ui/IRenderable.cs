using Cardamom.Graphics;
using Cardamom.Planar;

namespace Cardamom.Ui
{
    public interface IRenderable
    {
        void Draw(RenderTarget target, Transform2 transform);
        void Update(UiContext context, Transform2 transform, long delta);
    }
}
