namespace Cardamom.Ui.Controller
{
    public interface IFormElementController<T>
    {
        EventHandler<EventArgs>? ValueChanged { get; set; }

        T? GetValue();
        void SetValue(T? value);
    }
}
