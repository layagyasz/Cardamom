using Cardamom.Ui.Elements;

namespace Cardamom.Ui.Controller.Element
{
    public class SelectController<T> : ClassedUiElementController<Select>, IFormElementController<T>
    {
        public EventHandler<EventArgs>? ValueChanged { get; set; }

        private OptionElementController<T>? _selected;
        private T? _value;

        public T? GetValue()
        {
            return _value;
        }

        public void SetValue(T? value, bool notify = true)
        {
            foreach (var element in _element!.Cast<IUiElement>())
            {
                if (element.Controller is OptionElementController<T> controller)
                {
                    if (Equals(controller.Key, value))
                    {
                        SetSelected(controller, notify);
                        return;
                    }
                }
                else
                {
                    throw new InvalidOperationException("Selects may only contain Options.");
                }
            }
            throw new ArgumentException("No Option found for value.");
        }

        public override void Bind(object @object)
        {
            base.Bind(@object);
            _element!.ElementAdded += HandleElementAdded;
            _element!.ElementRemoved += HandleElementRemoved;
            foreach (var option in _element)
            {
                BindElement(option);
            }
            SetSelected(_element!.Cast<IUiElement>().FirstOrDefault()?.Controller, /* notify= */ true);
        }

        public override void Unbind()
        {
            _element!.ElementAdded -= HandleElementAdded;
            _element!.ElementRemoved -= HandleElementRemoved;
            foreach (var option in _element)
            {
                UnbindElement(option);
            }
            base.Unbind();
        }

        public override bool HandleMouseButtonClicked(MouseButtonClickEventArgs e)
        {
            Clicked?.Invoke(this, e);
            _element!.ToggleOpen();
            return true;
        }

        public override bool HandleMouseEntered()
        {
            SetHover(true);
            if (GetFocus())
            {
                _element!.SetOpen(true);
            }
            MouseEntered?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public override bool HandleMouseLeft()
        {
            SetHover(false);
            _element!.SetOpen(false);
            MouseLeft?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public override bool HandleFocusEntered()
        {
            SetFocus(true);
            Focused?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public override bool HandleFocusLeft()
        {
            SetFocus(false);
            _element!.SetOpen(false);
            FocusLeft?.Invoke(this, EventArgs.Empty);
            return true;
        }

        private void BindElement(IUiElement element)
        {
            var controller = (OptionElementController<T>)element.Controller;
            controller.Selected += HandleElementSelected;
            if ((_value == null && _selected == null) || (_value?.Equals(controller.Key) ?? false))
            {
                SetSelected(controller, /* notify= */ true);
            }
        }

        private void UnbindElement(IUiElement element)
        {
            var controller = (IOptionController<T>)element.Controller;
            controller.Selected -= HandleElementSelected;
            if (controller == _selected)
            {
                SetSelected(
                    _element!
                        .Cast<IUiElement>()
                        .Select(x => x.Controller)
                        .Cast<OptionElementController<T>>()
                        .FirstOrDefault(),
                    /* notify= */ true);
            }
        }

        private void SetSelected(IElementController? elementController, bool notify)
        {
            _selected?.SetSelected(false);
            if (elementController == null)
            {
                _selected = null;
                _value = default;
                if (notify)
                {
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            else if (elementController is SelectOptionElementController<T> controller)
            {
                _selected?.SetSelected(false);
                controller.SetSelected(true);
                _element!.SetText(controller.GetText());
                _selected = controller;
                _value = controller.Key;
                if (notify)
                {
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                throw new InvalidOperationException("Selects may only contain Options.");
            }
        }

        private void HandleElementAdded(object? sender, ElementEventArgs e)
        {
            BindElement((IUiElement)e.Element);
        }

        private void HandleElementRemoved(object? sender, ElementEventArgs e)
        {
            UnbindElement((IUiElement)e.Element);
        }

        private void HandleElementSelected(object? sender, EventArgs e)
        {
            SetSelected((IElementController)sender!, /* notify= */ true);
        }
    }
}
