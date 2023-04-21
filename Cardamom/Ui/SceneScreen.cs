using Cardamom.Graphics;
using Cardamom.Ui.Controller;
using Cardamom.Ui.Elements;
using OpenTK.Mathematics;

namespace Cardamom.Ui
{
    public class SceneScreen : Screen
    {
        public IScene Scene { get; }

        public SceneScreen(IController controller, IEnumerable<UiGroup> uiLayers, IScene scene)
            : base(controller, uiLayers)
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

        public override void Draw(IRenderTarget target, IUiContext context)
        {
            Scene.Draw(target, context);
            target.Flatten();
            context.Flatten();
            foreach (var layer in UiLayers)
            {
                layer.Draw(target, context);
            }
        }

        public override void Update(long delta)
        {
            Scene.Update(delta);
            base.Update(delta);
        }
    }
}
