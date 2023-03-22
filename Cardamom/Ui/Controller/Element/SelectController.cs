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
            foreach (var element in _element!.GetDropBox().Cast<IUiElement>())
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
            if (_element!.GetDropBox().Controller is TableController dropBoxController)
            {
                dropBoxController.ElementClicked = HandleElementClicked;
            }
            SetSelected(_element!.GetDropBox().Cast<IUiElement>().First().Controller);
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

        private void SetSelected(IElementController elementController)
        {
            if (elementController is SelectOptionElementController<T> controller)
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

        private void HandleElementClicked(object? sender, ElementClickedEventArgs e)
        {
            SetSelected(e.Element);
        }
    }
}
