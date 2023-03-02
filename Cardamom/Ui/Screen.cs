using Cardamom.Graphics;
using Cardamom.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Ui
{
    public class Screen : IRenderable
    {
        public IController Controller { get; }

        public IUiGroup[] UiLayers { get; }

        public Screen(IController controller, IEnumerable<IUiGroup> uiLayers)
        {
            Controller = controller;
            UiLayers = uiLayers.ToArray();
        }

        public virtual void Initialize()
        {
            Controller.Bind(this);
            foreach (var layer in UiLayers)
            {
                layer.Initialize();
            }
        }

        public virtual void ResizeContext(Vector3 bounds) { }

        public virtual void Draw(RenderTarget target, UiContext context)
        {
            foreach (var layer in UiLayers)
            {
                layer.Draw(target, context);
            }
        }

        public virtual void Update(long delta)
        {
            foreach (var layer in UiLayers)
            {
                layer.Update(delta);
            }
        }
    }
}
