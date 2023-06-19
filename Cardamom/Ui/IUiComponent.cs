using Cardamom.Ui.Controller;

namespace Cardamom.Ui
{
    public interface IUiComponent : IUiElement
    {
        IController ComponentController { get; }
    }
}
