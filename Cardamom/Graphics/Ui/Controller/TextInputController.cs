using Cardamom.Collections;
using Cardamom.Graphics.Ui.Elements;
using Cardamom.Window;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Graphics.Ui.Controller
{
    public class TextInputController 
        : ClassedUiElementController<EditableTextUiElement>, IFormElementController<string, string>
    {
        private static readonly EnumSet<Keys> DISREGARD_KEYS = new() { Keys.Enter, Keys.Tab };

        public EventHandler<ValueChangedEventArgs<string, string>>? ValueChanged { get; set; }

        public string Key { get; }

        private readonly IKeyMapper _keyMapper;

        private string _value = string.Empty;
        private int _cursor = 0;

        public TextInputController(string key, IKeyMapper keyMapper)
        {
            Key = key;
            _keyMapper = keyMapper;
        }

        public string? GetValue()
        {
            return _value;
        }

        public void SetValue(string? value)
        {
            _value = value ?? string.Empty;
            _element!.SetText(_value);
            ValueChanged?.Invoke(this, new(Key, _value));
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
            _element!.SetCursorActive(true);
            SetCursor(0);
            return true;
        }

        public override bool HandleFocusLeft()
        {
            SetFocus(false);
            _element!.SetCursorActive(false);
            SetCursor(0);
            return true;
        }

        public override bool HandleKeyPressed(KeyboardKeyEventArgs e)
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
                    SetValue(_value[..(_cursor - 1)] + _value[_cursor..]);
                    MoveCursor(-1);
                }
            }
            else if(!DISREGARD_KEYS.Contains(e.Key))
            {
                string text = _keyMapper.Map(e);
                if (text.Length > 0)
                {
                    string newValue = _value[.._cursor] + text + _value[_cursor..];
                    SetValue(newValue);
                    MoveCursor(text.Length);
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
