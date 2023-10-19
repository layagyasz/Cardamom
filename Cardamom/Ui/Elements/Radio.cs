using Cardamom.Ui.Controller;

namespace Cardamom.Ui.Elements
{
    public class Radio : UiCompoundComponent
    {
        public class Style
        {
            public string? Container { get; set; }
            public string? Option { get; set; }
        }

        public Radio(IController controller, IUiContainer container)
            : base(controller, container) { }
    }
}
