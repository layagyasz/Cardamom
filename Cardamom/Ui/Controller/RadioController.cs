using Cardamom.Ui.Controller.Element;
using Cardamom.Ui.Elements;

namespace Cardamom.Ui.Controller
{
    public class RadioController<T> : IController, IFormElementController<string, T>
    {
        public EventHandler<ValueChangedEventArgs<string, T>>? ValueChanged { get; set; }

        public string Key { get; }

        UiComponent? _element;
        private OptionController<T>? _selected;
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
            foreach (var element in _element!.Cast<IUiElement>())
            {
                if (element.Controller is OptionController<T> controller)
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
            _element = (UiComponent)@object;
            foreach (var option in _element.Cast<IUiElement>())
            {
                if (option.Controller is OptionController<T> controller)
                {
                    controller.Clicked += HandleElementClicked;
                }
            }
            SetSelected(
                _element!.Cast<IUiElement>().Where(x => x.Controller is OptionController<T>).First().Controller);
        }

        public void Unbind()
        {
            foreach (var option in _element!.Cast<IUiElement>())
            {
                if (option is OptionController<T> controller)
                {
                    controller.Clicked -= HandleElementClicked;
                }
            }
            _element = null;
        }

        private void SetSelected(IElementController elementController)
        {
            if (elementController is OptionController<T> controller)
            {
                _selected?.SetValue(false);
                controller.SetValue(true);
                _selected = controller;
                _value = controller.Key;
                ValueChanged?.Invoke(this, new ValueChangedEventArgs<string, T>(Key, _value));
            }
        }

        private void HandleElementClicked(object? sender, MouseButtonClickEventArgs e)
        {
            SetSelected((IElementController)sender!);
        }
    }
}
