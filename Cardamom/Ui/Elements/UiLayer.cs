using Cardamom.Ui.Controller;

namespace Cardamom.Ui.Elements
{
    public class UiLayer : UiGroup, IControlled
    {
        public IController Controller { get; }
        public IControlled? Parent { get; set; }

        public UiLayer(IController controller)
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
