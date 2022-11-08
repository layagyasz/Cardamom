using SFML.Graphics;

namespace Cardamom.Ui
{
    public interface IRenderable
    {
        void Draw(RenderTarget target, Transform transform);
        void Update(UiContext context, Transform transform, long delta);
    }
}
