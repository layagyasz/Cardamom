using Cardamom.Graphics.Ui;
using Cardamom.Planar;

namespace Cardamom.Graphics
{
    public interface IRenderable : IInitializable
    {
        void Draw(RenderTarget target);
        void Update(UiContext context, long delta);
    }
}
