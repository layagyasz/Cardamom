namespace Cardamom.Ui.Controller.Element
{
    public interface IPaneController : IElementController
    {
        EventHandler<EventArgs>? Closed { get; set; }

        IUiElement GetPane();
    }
}
