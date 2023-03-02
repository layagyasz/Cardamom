using Cardamom.Ui.Elements;

namespace Cardamom.Ui.Controller.Element
{
    public class OptionController<T> : ClassedUiElementController<TextUiElement>, IFormElementController<T, bool>
    {
        public EventHandler<ValueChangedEventArgs<T, bool>>? ValueChanged { get; set; }

        public T Key { get; }

        private bool _value;

        public OptionController(T key)
        {
            Key = key;
        }

        public string GetText()
        {
            return _element!.GetText();
        }

        public bool GetValue()
        {
            return _value;
        }

        public void SetValue(bool selected)
        {
            _value = selected;
            SetToggle(selected);
            ValueChanged?.Invoke(this, new ValueChangedEventArgs<T, bool>(Key, _value));
        }

        public override bool HandleMouseEntered()
        {
            SetHover(true);
            return true;
        }

        public override bool HandleMouseLeft()
        {
            SetHover(false);
            return true;
        }

        public override bool HandleFocusEntered()
        {
            SetFocus(true);
            return true;
        }

        public override bool HandleFocusLeft()
        {
            SetFocus(false);
            return true;
        }
    }
}
