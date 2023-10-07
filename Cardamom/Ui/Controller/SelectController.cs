using Cardamom.Ui.Elements;

namespace Cardamom.Ui.Controller
{
    public class SelectController<T> : IController, IRandomizableFormFieldController<T>
    {
        private static readonly string s_DefaultText = "Click to select";

        public EventHandler<EventArgs>? ValueChanged { get; set; }

        private List<SelectOption<T>> _range;

        private Select? _component;
        private IRandomizableFormFieldController<T>? _optionsContainer;
        private SelectOption<T>? _value;

        private bool _focus;

        public SelectController(IEnumerable<SelectOption<T>> range, T? initialValue)
        {
            _range = range.ToList();
            if (initialValue != null)
            {
                _value = range.First(x => Equals(x, initialValue));
            }
        }

        public void Bind(object @object)
        {
            _component = (Select)@object;
            _component.Root.Controller.Focused += HandleFocused;
            _component.Root.Controller.FocusLeft += HandleFocusLeft;
            _component.Root.Controller.MouseEntered += HandleMouseEntered;
            _component.Root.Controller.MouseLeft += HandleMouseLeft;
            _optionsContainer = (IRandomizableFormFieldController<T>)_component.Options.ComponentController;
            _optionsContainer.ValueChanged += HandleValueChanged;

            foreach (var option in _range)
            {
                _component.AddOption(option);
            }
            if (_value != null)
            {
                _optionsContainer.SetValue(_value.Value, /* notify= */ false);
            }
            UpdateText();
        }

        public void Unbind()
        {
            _component!.Root.Controller.Focused -= HandleFocused;
            _component!.Root.Controller.FocusLeft -= HandleFocusLeft;
            _component!.Root.Controller.MouseEntered -= HandleMouseEntered;
            _component!.Root.Controller.MouseLeft -= HandleMouseLeft;
            _component = null;
            _optionsContainer!.ValueChanged -= HandleValueChanged;
            _optionsContainer = null;
        }

        public T? GetValue()
        {
            return _value == null ? default : _value.Value;
        }

        public void Randomize(Random random, bool notify = true)
        {
            _optionsContainer!.Randomize(random, notify);
            _value = _range.First(x => Equals(x.Value, _optionsContainer!.GetValue()));
            UpdateText();
            if (notify)
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SetRange(IEnumerable<SelectOption<T>> range)
        {
            _range = range.ToList();
            if (_component != null)
            {
                var oldValue = _value;
                _component!.Clear();
                foreach (var option in range)
                {
                    _component.AddOption(option);
                }
                SetValue(oldValue == null ? default : oldValue.Value);
            }
        }

        public void SetValue(T? value, bool notify = true)
        {
            if (value == null)
            {
                _value = null;
                _optionsContainer!.SetValue(default, /* notify= */ false);
            }   
            else
            {
                _value = _range.FirstOrDefault(x => Equals(x.Value, value));
                _optionsContainer!.SetValue(_value == null ? default : _value.Value, /* notify= */ false);
            }
            UpdateText();
            if (notify)
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void HandleFocused(object? sender, EventArgs e)
        {
            _component!.SetOpen(true);
            _focus = true;
        }

        private void HandleFocusLeft(object? sender, EventArgs e)
        {
            _component!.SetOpen(false);
            _focus = false;
        }

        private void HandleMouseEntered(object? sender, EventArgs e)
        {
            _component!.SetOpen(_focus);
        }

        private void HandleMouseLeft(object? sender, EventArgs e)
        {
            _component!.SetOpen(false);
        }

        private void HandleValueChanged(object? sender, EventArgs e)
        {
            _value = _range.FirstOrDefault(x => Equals(x.Value, _optionsContainer!.GetValue()));
            UpdateText();
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateText()
        {
            _component!.Root.SetText(_value?.Text ?? s_DefaultText);
        }
    }
}
