using Cardamom.Collections;
using Cardamom.Graphics.Ui.Elements;
using Cardamom.Trackers;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Graphics.Ui.Controller
{
    public class TextInputController 
        : ClassedUiElementController<EditableTextUiElement>, IFormElementController<string, string>
    {
        private static readonly EnumMap<Keys, char> KEY_MAP =
            new()
            {
                {Keys.A, 'a' },
                {Keys.B, 'b' },
                {Keys.C, 'c' },
                {Keys.D, 'd' },
                {Keys.E, 'e' },
                {Keys.F, 'f' },
                {Keys.G, 'g' },
                {Keys.H, 'h' },
                {Keys.I, 'i' },
                {Keys.J, 'j' },
                {Keys.J, 'k' },
                {Keys.K, 'k' },
                {Keys.L, 'l' },
                {Keys.M, 'm' },
                {Keys.N, 'n' },
                {Keys.O, 'o' },
                {Keys.P, 'p' },
                {Keys.Q, 'q' },
                {Keys.R, 'r' },
                {Keys.S, 's' },
                {Keys.T, 't' },
                {Keys.U, 'u' },
                {Keys.V, 'v' },
                {Keys.W, 'w' },
                {Keys.X, 'x' },
                {Keys.Y, 'y' },
                {Keys.Z, 'z' },
                {Keys.Space, ' ' },
                {Keys.D1, '1' }
            };

        public EventHandler<ValueChangedEventArgs<string, string>>? ValueChanged { get; set; }

        public string Key { get; }

        private string _value = string.Empty;
        private int _cursor = 0;

        public TextInputController(string key)
        {
            Key = key;
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
            else
            {
                string text = ToText(e);
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

        private static string ToText(KeyboardKeyEventArgs e)
        {
            if (!KEY_MAP.ContainsKey(e.Key))
            {
                return string.Empty;
            }
            char c = KEY_MAP[e.Key];
            if (e.Shift)
            {
                return char.ToUpper(c).ToString();
            }
            return c.ToString();
        }
    }
}
