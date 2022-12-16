using Cardamom.Graphics.Ui.Controller;

namespace Cardamom.Graphics.Ui.Elements
{
    public class UiGroupLayer : UiGroup, IUiLayer
    {
        public IController Controller { get; }
        public IControlled? Parent { get; set; }

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
