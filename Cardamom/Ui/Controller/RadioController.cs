using Cardamom.Ui.Controller.Element;
using Cardamom.Ui.Elements;

namespace Cardamom.Ui.Controller
{
    public class RadioController<T> : IController, IFormElementController<string, T>
    {
        public EventHandler<ValueChangedEventArgs<string, T?>>? ValueChanged { get; set; }

        public string Key { get; }

        UiCompoundComponent? _element;
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
            foreach (var element in _element!.Cast<IUiElement>())
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

        public void Bind(object @object)
        {
            _element = (UiCompoundComponent)@object;
            _element.ElementAdded += HandleElementAdded;
            _element.ElementRemoved += HandleElementRemoved;
            foreach (var option in _element.Cast<IUiElement>())
            {
                BindElement(option);
            }
            var selected = 
                _element!.Cast<IUiElement>().Where(x => x.Controller is OptionElementController<T>).FirstOrDefault();
            if (selected != null)
            {
                SetSelected(selected.Controller);
            }
        }

        public void Unbind()
        {
            foreach (var option in _element!.Cast<IUiElement>())
            {
                UnbindElement(option);
            }
            _element!.ElementAdded -= HandleElementAdded;
            _element!.ElementRemoved -= HandleElementRemoved;
            _element = null;
        }

        private void BindElement(IUiElement element)
        {
            if (element.Controller is IOptionController<T> controller)
            {
                controller.Clicked += HandleElementClicked;
            }
        }

        private void UnbindElement(IUiElement element)
        {
            if (element.Controller is IOptionController<T> controller)
            {
                controller.Clicked -= HandleElementClicked;
            }
        }

        private void SetSelected(IElementController? elementController)
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

        private void HandleElementAdded(object? sender, ElementEventArgs e)
        {
            BindElement((IUiElement)e.Element);
        }

        private void HandleElementClicked(object? sender, MouseButtonClickEventArgs e)
        {
            SetSelected((IElementController)sender!);
        }

        private void HandleElementRemoved(object? sender, ElementEventArgs e)
        {
            UnbindElement((IUiElement)e.Element);
        }
    }
}
