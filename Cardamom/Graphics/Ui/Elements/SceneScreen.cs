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

        public override void Initialize()
        {
            base.Initialize();
            Scene.Initialize();
        }

        public override void Draw(RenderTarget target)
        {
            target.PushTranslation(Position);
            Scene.Draw(target);
            target.PopViewMatrix();
            target.Flatten();
            base.Draw(target);
        }

        public override void Update(UiContext context, long delta)
        {
            context.Register(this);

            context.PushTranslation(Position);
            Scene.Update(context, delta);
            context.Flatten();

            foreach (var layer in _uiLayers)
            {
                layer.Update(context, delta);
            }
            context.PopViewMatrix();
        }
    }
}
