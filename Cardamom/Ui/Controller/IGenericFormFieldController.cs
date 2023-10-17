namespace Cardamom.Ui.Controller
{
    public interface IGenericFormFieldController : IController
    {
        EventHandler<EventArgs>? ValueChanged { get; set; }

        object? Get();
        void Set(object? value, bool notify = true);
    }
}
