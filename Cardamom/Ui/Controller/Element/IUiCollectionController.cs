namespace Cardamom.Ui.Controller.Element
{
    public interface IUiCollectionController : IElementController
    {
        EventHandler<ElementEventArgs>? ElementClicked { get; set; }
    }
}
