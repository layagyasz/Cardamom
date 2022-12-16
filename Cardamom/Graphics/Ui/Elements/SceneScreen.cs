using Cardamom.Graphics.Ui.Controller;
using Cardamom.Planar;

namespace Cardamom.Graphics.Ui.Elements
{
    public class SceneScreen : Screen
    {
        private readonly Scene _scene;

        public SceneScreen(Rectangle bounds, IController controller, IEnumerable<IUiLayer> uiLayers, Scene scene)
            : base(bounds, controller, uiLayers)
        {
            _scene = scene;
        }

        public override void Draw(RenderTarget target)
        {
            _scene.Draw(target);
            base.Draw(target);
        }

        public override void Update(UiContext context, long delta)
        {
            _scene.Update(context, delta);
            base.Update(context, delta);
        }
    }
}
