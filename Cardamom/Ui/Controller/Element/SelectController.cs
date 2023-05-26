﻿using Cardamom.Ui.Elements;

namespace Cardamom.Ui.Controller.Element
{
    public class SelectController<T> : ClassedUiElementController<Select>, IFormElementController<string, T>
    {
        public EventHandler<ValueChangedEventArgs<string, T?>>? ValueChanged { get; set; }

        public string Key { get; }

        private OptionElementController<T>? _selected;
        private T? _value;

        public SelectController(string key)
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
                if (element.Controller is OptionElementController<T> controller)
                {
                    if (Equals(controller.Key, value))
                    {
                        SetSelected(controller);
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
            SetSelected(_element!.Cast<IUiElement>().FirstOrDefault()?.Controller);
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
            return true;
        }

        public override bool HandleMouseLeft()
        {
            SetHover(false);
            _element!.SetOpen(false);
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
            _element!.SetOpen(false);
            return true;
        }

        private void BindElement(IUiElement element)
        {
            var controller = (OptionElementController<T>)element.Controller;
            controller.Selected += HandleElementSelected;
            if ((_value == null && _selected == null) || (_value?.Equals(controller.Key) ?? false))
            {
                SetSelected(controller);
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
                        .FirstOrDefault());
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
            else if (elementController is SelectOptionElementController<T> controller)
            {
                _selected?.SetSelected(false);
                controller.SetSelected(true);
                _element!.SetText(controller.GetText());
                _selected = controller;
                _value = controller.Key;
                ValueChanged?.Invoke(this, new(Key, _value));
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
            SetSelected((IElementController)sender!);
        }
    }
}
