namespace Cardamom.Ui.Controller
{
    public interface IFormFieldController<T> : IGenericFormFieldController
    {
        T? GetValue();
        void SetValue(T? value, bool notify = true);

        object? IGenericFormFieldController.Get()
        {
            return GetValue();
        }

        void IGenericFormFieldController.Set(object? value, bool notify)
        {
            SetValue((T?)value, notify);
        }
    }
}
