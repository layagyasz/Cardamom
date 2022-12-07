namespace Cardamom.Graphics.Ui.Controller
{
    public interface IFormElementController<TKey, TValue> : IController
    {
        EventHandler<ValueChangedEventArgs<TKey, TValue>>? ValueChanged { get; set; }

        TKey Key { get; }

        TValue? GetValue();
        void SetValue(TValue? value);
    }
}
