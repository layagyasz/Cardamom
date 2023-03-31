using Cardamom.Graphics;

namespace Cardamom.Ui.Controller.Element
{
    public interface IControlledElement : IRenderable
    {
        IElementController Controller { get; }
        IControlledElement? Parent { get; set; }
    }
}
