namespace Cardamom.Ui.Controller.Element
{
    public interface IUiCollectionController : IElementController
    {
        EventHandler<ElementClickedEventArgs>? ElementClicked { get; set; }
    }
}
