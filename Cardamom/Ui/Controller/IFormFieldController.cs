namespace Cardamom.Ui.Controller
{
    public interface IFormFieldController<T> : IController
    {
        EventHandler<EventArgs>? ValueChanged { get; set; }

        T? GetValue();
        void SetValue(T? value, bool notify = true);
    }
}
