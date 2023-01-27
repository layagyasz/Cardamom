using Cardamom.Graphics.Ui.Controller;
using Cardamom.Mathematics.Geometry;
using OpenTK.Mathematics;

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

        public override void ResizeContext(Vector3 bounds)
        {
            Scene.ResizeContext(bounds);
        }

        public override void Draw(RenderTarget target, UiContext context)
        {
            context.Register(this);
            target.PushTranslation(Position);
            Scene.Draw(target, context);
            target.Flatten();
            context.Flatten();
            foreach (var layer in _uiLayers)
            {
                layer.Draw(target, context);
            }
            target.PopModelMatrix();
        }

        public override void Update(long delta)
        {
            Scene.Update(delta);
            base.Update(delta);
        }
    }
}
