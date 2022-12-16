using Cardamom.Graphics.Ui.Controller;
using Cardamom.Mathematics.Geometry;

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
            target.PopViewMatrix();
            base.Draw(target);
        }

        public override void Update(UiContext context, long delta)
        {
            context.PushTranslation(Position);
            Scene.Update(context, delta);
            context.PopViewMatrix();
            base.Update(context, delta);
        }
    }
}
