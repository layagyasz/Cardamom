﻿using Cardamom.Collections;
using Cardamom.Ui.Elements;
using Cardamom.Window;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Ui.Controller.Element
{
    public class TextInputController : ClassedUiElementController<EditableTextUiElement>, IFormFieldController<string>
    {
        private static readonly EnumSet<Keys> s_DisregardKeys = new() { Keys.Enter, Keys.Tab };

        public EventHandler<EventArgs>? ValueChanged { get; set; }

        private string _value = string.Empty;
        private int _cursor = 0;

        public string? GetValue()
        {
            return _value;
        }

        public void SetValue(string? value, bool notify = true)
        {
            _value = value ?? string.Empty;
            _element!.SetText(_value);
            if (notify)
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public override bool HandleMouseButtonClicked(MouseButtonClickEventArgs e)
        {
            Clicked?.Invoke(this, e);
            return true;
        }

        public override bool HandleMouseEntered()
        {
            SetHover(true);
            MouseEntered?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public override bool HandleMouseLeft()
        {
            SetHover(false);
            MouseEntered?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public override bool HandleFocusEntered()
        {
            SetFocus(true);
            _element!.SetCursorActive(true);
            SetCursor(0);
            Focused?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public override bool HandleFocusLeft()
        {
            SetFocus(false);
            _element!.SetCursorActive(false);
            SetCursor(0);
            FocusLeft?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public override bool HandleTextEntered(TextEnteredEventArgs e)
        {
            if (e.Key == Keys.Left)
            {
                MoveCursor(-1);
            }
            else if (e.Key == Keys.Right)
            {
                MoveCursor(1);
            }
            else if (e.Key == Keys.Up)
            {
                SetCursor(_value.Length);
            }
            else if (e.Key == Keys.Down)
            {
                SetCursor(0);
            }
            else if (e.Key == Keys.Backspace)
            {
                if (_cursor > 0)
                {
                    SetValue(_value[..(_cursor - 1)] + _value[_cursor..], /* notify= */ true);
                    MoveCursor(-1);
                }
            }
            else if (!s_DisregardKeys.Contains(e.Key))
            {
                if (e.Text.Length > 0)
                {
                    string newValue = _value[.._cursor] + e.Text + _value[_cursor..];
                    SetValue(newValue, /* notify= */ true);
                    MoveCursor(e.Text.Length);
                }
            }
            return true;
        }

        private void MoveCursor(int amount)
        {
            SetCursor(_cursor + amount);
        }

        private void SetCursor(int index)
        {
            _cursor = Math.Min(Math.Max(index, 0), _value.Length);
            _element!.SetCursor(_cursor);
        }
    }
}
