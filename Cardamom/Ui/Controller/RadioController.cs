using Cardamom.Ui.Elements;

namespace Cardamom.Ui.Controller
{
    public class RadioController<T> : DynamicComponentControllerBase, IController, IFormElementController<T>
    {
        public EventHandler<EventArgs>? ValueChanged { get; set; }

        private IOptionController<T>? _selected;
        private T? _value;

        public RadioController() { }

        public RadioController(T? initialValue, int bindRecursionDepth = 0)
            : base(bindRecursionDepth)
        {
            _value = initialValue;
        }

        public T? GetValue()
        {
            return _value;
        }

        public void SetValue(T? value)
        {
            if (Equals(value, _value))
            {
                return;
            }
            if (value == null)
            {
                SetSelected(null);
                return;
            }
            foreach (var element in GetChildren().Cast<IUiElement>())
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

        protected override void BindElement(IUiElement element)
        {
            foreach (var controller in GetControllers(element))
            {
                if (controller is IOptionController<T> option)
                {
                    option.Selected += HandleElementSelected;
                    if ((_value == null && _selected == null) || (_value?.Equals(option.Key) ?? false))
                    {
                        SetSelected(option);
                    }
                }
            }
        }

        protected override void UnbindElement(IUiElement element)
        {
            foreach (var controller in GetControllers(element))
            {
                if (controller is IOptionController<T> option)
                {
                    option.Selected -= HandleElementSelected;
                    if (option == _selected)
                    {
                        SetSelected(
                            GetChildren()
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
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
            else if (elementController is IOptionController<T> controller)
            {
                controller.SetSelected(true);
                _selected = controller;
                _value = controller.Key;
                ValueChanged?.Invoke(this, EventArgs.Empty);
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
