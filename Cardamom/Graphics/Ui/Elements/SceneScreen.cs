using Cardamom.Graphics.Ui.Controller;
using Cardamom.Planar;

namespace Cardamom.Graphics.Ui.Elements
{
    public class SceneScreen : Screen
    {
        public Scene Scene { get; }

        public SceneScreen(Rectangle bounds, IController controller, IEnumerable<IUiLayer> uiLayers, Scene scene)
            : base(bounds, controller, uiLayers)
        {
            Scene = scene;
        }

        public override void Draw(RenderTarget target)
        {
            target.PushTranslation(Position);
            Scene.Draw(target);
            target.PopTransform();
            base.Draw(target);
        }

        public override void Update(UiContext context, long delta)
        {
            context.PushTranslation(Position);
            Scene.Update(context, delta);
            context.PopTransform();
            base.Update(context, delta);
        }
    }
}
