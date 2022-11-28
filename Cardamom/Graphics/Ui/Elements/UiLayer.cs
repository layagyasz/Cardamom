using Cardamom.Graphics.Ui.Controller;

namespace Cardamom.Graphics.Ui.Elements
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
