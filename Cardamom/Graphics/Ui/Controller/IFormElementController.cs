namespace Cardamom.Graphics.Ui.Controller
{
    public interface IFormElementController<TKey, TValue> : IController
    {
        TKey Key { get; }

        TValue? GetValue();
        void SetValue(TValue? value);
    }
}
