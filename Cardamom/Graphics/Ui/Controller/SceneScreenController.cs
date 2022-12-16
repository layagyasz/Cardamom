using Cardamom.Graphics.Ui.Elements;

namespace Cardamom.Graphics.Ui.Controller
{
    public class SceneScreenController : PassthroughController
    {
        protected SceneScreen? _screen;

        public override void Bind(object @object)
        {
            base.Bind(@object);
            _screen = (SceneScreen)@object;
            SetSubcontroller(_screen.Scene.Controller);
        }

        public override void Unbind()
        {
            _screen = default;
            base.Unbind();
        }
    }
}
