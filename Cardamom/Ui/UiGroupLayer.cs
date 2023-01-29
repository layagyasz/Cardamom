using Cardamom.Ui.Controller;
using Cardamom.Ui.Elements;

namespace Cardamom.Ui
{
    public class UiGroupLayer : UiGroup, IUiLayer
    {
        public IController Controller { get; }

        public UiGroupLayer(IController controller)
        {
            Controller = controller;
        }

        public override void Initialize()
        {
            base.Initialize();
            Controller.Bind(this);
        }
    }
}
