namespace Cardamom.Graphics.Ui.Controller
{
    public interface IUiCollectionController : IController
    {
        EventHandler<ElementEventArgs>? ElementClicked { get; set; }
    }
}
