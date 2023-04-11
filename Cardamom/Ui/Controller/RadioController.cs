using Cardamom.Ui.Elements;

namespace Cardamom.Ui.Controller
{
    public class RadioController<T> : DynamicComponentControllerBase, IController, IFormElementController<string, T>
    {
        public EventHandler<ValueChangedEventArgs<string, T?>>? ValueChanged { get; set; }

        public string Key { get; }

        private IOptionController<T>? _selected;
        private T? _value;

        public RadioController(string key)
        {
            Key = key;
        }

        public T? GetValue()
        {
            return _value;
        }

        public void SetValue(T? value)
        {
            if (value == null)
            {
                SetSelected(null);
                return;
            }
            foreach (var element in _component!.Cast<IUiElement>())
            {
                if (element.Controller is IOptionController<T> controller)
                {
                    if (Equals(controller.Key, value))
                    {
                        SetSelected(controller);
                        return;
                    }
                }
            }
            throw new ArgumentException("No Option found for value.");
        }

        public override void BindElement(IUiElement element)
        {
            foreach (var controller in GetControllers(element))
            {
                if (controller is IOptionController<T> option)
                {
                    option.Selected += HandleElementSelected;
                    if (_selected == null)
                    {
                        SetSelected(option);
                    }
                }
            }
        }

        public override void UnbindElement(IUiElement element)
        {
            foreach (var controller in GetControllers(element))
            {
                if (controller is IOptionController<T> option)
                {
                    option.Selected -= HandleElementSelected;
                    if (option == _selected)
                    {
                        SetSelected(
                            _component!
                                .SelectMany(GetControllers)
                                .Where(x => x is IOptionController<T>)
                                .Cast<IOptionController<T>>()
                                .FirstOrDefault());
                    }
                }
            }
        }

        private void SetSelected(IOptionController<T>? elementController)
        {
            _selected?.SetSelected(false);
            if (elementController == null)
            {
                _selected = null;
                _value = default;
                ValueChanged?.Invoke(this, new(Key, _value));
            }
            else if (elementController is IOptionController<T> controller)
            {
                controller.SetSelected(true);
                _selected = controller;
                _value = controller.Key;
                ValueChanged?.Invoke(this, new(Key, _value));
            }
        }

        private void HandleElementSelected(object? sender, EventArgs e)
        {
            SetSelected((IOptionController<T>)sender!);
        }
        
        private static IEnumerable<IController> GetControllers(IUiElement element)
        {
            yield return element.Controller;
            if (element is UiCompoundComponent compound)
            {
                yield return compound.ComponentController;
            }
        }
    }
}
