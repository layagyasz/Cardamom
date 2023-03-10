namespace Cardamom.Ui.Controller.Element
{
    public interface IOptionController<T> : IElementController
    {
        T Key { get; }
        void SetSelected(bool selected);
    }
}
