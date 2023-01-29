namespace Cardamom.Ui.Controller.Element
{
    public interface IControlledElement
    {
        IElementController Controller { get; }
        IControlledElement? Parent { get; set; }
    }
}
