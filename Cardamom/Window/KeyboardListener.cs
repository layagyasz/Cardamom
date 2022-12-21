using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Window
{
    public class KeyboardListener
    {
        public EventHandler<TextEnteredEventArgs>? TextEntered { get; set; }
        public EventHandler<KeyDownEventArgs>? KeyDown { get; set; }

        private readonly IKeyMapper _keyMapper;
        private readonly Keys[] _keysToCheck;

        private RenderWindow? _window;

        public KeyboardListener(IKeyMapper keyMapper, Keys[] keysToCheck)
        {
            _keyMapper = keyMapper;
            _keysToCheck = keysToCheck;
        }

        public void Bind(RenderWindow window)
        {
            _window = window;
            _window.KeyPressed += HandleKeyPressed;
        }

        public void DispatchEvents(long delta)
        {
            if (!_window!.IsAnyKeyDown())
            {
                return;
            }

            var modifiers = GetModifiers();
            foreach (var key in _keysToCheck)
            {
                if (_window!.IsKeyDown(key))
                {
                    KeyDown?.Invoke(
                        this, 
                        new() 
                        { 
                            Key = key, 
                            Modifiers = modifiers, 
                            ScanCode = 0,
                            TimeDelta = delta 
                        });
                }
            }
        }

        private KeyModifiers GetModifiers()
        {
            KeyModifiers modifiers = 0;
            if (_window!.IsKeyDown(Keys.LeftShift) || _window!.IsKeyDown(Keys.RightShift))
            {
                modifiers |= KeyModifiers.Shift;
            }
            if (_window!.IsKeyDown(Keys.LeftControl) || _window!.IsKeyDown(Keys.RightControl))
            {
                modifiers |= KeyModifiers.Control;
            }
            if (_window!.IsKeyDown(Keys.LeftAlt) || _window!.IsKeyDown(Keys.RightAlt))
            {
                modifiers |= KeyModifiers.Alt;  
            }
            if (_window!.IsKeyDown(Keys.LeftSuper) || _window!.IsKeyDown(Keys.RightSuper))
            {
                modifiers |= KeyModifiers.Super;
            }
            if (_window!.IsKeyDown(Keys.CapsLock))
            {
                modifiers |= KeyModifiers.CapsLock;
            }
            if (_window!.IsKeyDown(Keys.NumLock))
            {
                modifiers |= KeyModifiers.NumLock;
            }
            return modifiers;
        }

        private void HandleKeyPressed(object? sender, KeyboardKeyEventArgs e)
        {
            var args =
                new TextEnteredEventArgs()
                {
                    IsRepeat = e.IsRepeat,
                    Key = e.Key,
                    Modifiers = e.Modifiers,
                    ScanCode = e.ScanCode,
                    Text = _keyMapper.Map(e)
                };
            TextEntered?.Invoke(this, args);
        }
    }
}
