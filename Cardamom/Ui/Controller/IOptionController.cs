namespace Cardamom.Ui.Controller
{
    public interface IOptionController<T> : IController
    {
        EventHandler<EventArgs>? Selected { get; set; }
        T Key { get; }
        void SetSelected(bool selected);
    }
}
