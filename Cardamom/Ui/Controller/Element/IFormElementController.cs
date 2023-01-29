namespace Cardamom.Ui.Controller.Element
{
    public interface IFormElementController<TKey, TValue> : IElementController
    {
        EventHandler<ValueChangedEventArgs<TKey, TValue>>? ValueChanged { get; set; }

        TKey Key { get; }

        TValue? GetValue();
        void SetValue(TValue? value);
    }
}
