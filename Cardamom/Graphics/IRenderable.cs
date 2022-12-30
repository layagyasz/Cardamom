using Cardamom.Graphics.Ui;

namespace Cardamom.Graphics
{
    public interface IRenderable : IInitializable
    {
        void Draw(RenderTarget target, UiContext context);
        void Update(long delta);
    }
}
