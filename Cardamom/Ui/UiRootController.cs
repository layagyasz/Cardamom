using Cardamom.Ui.Controller;
using SFML.Graphics;
using SFML.Window;

namespace Cardamom.Ui
{
    public class UiRootController
    {
        private UiContext? _context;
        private IControlled? _focus;
        private IControlled? _mouseOver;

        public void Bind(RenderWindow window)
        {
            window.KeyPressed += HandleKeyPressed;
            window.TextEntered += HandleTextEntered;
        }

        public void Bind(MouseListener mouseListener)
        {
            mouseListener.MouseButtonClicked += HandleMouseButtonClicked;
            mouseListener.MouseWheelScrolled += HandleMouseWheelScrolled;
        }

        public void Bind(UiContext context)
        {
            _context = context;
        }

        public void DispatchEvents()
        {
            if (_context == null)
            {
                return;
            }

            if (_context?.GetTopElement() != _mouseOver)
            {
                _mouseOver?.Controller?.HandleMouseLeft();
                _mouseOver = _context?.GetTopElement();
                _mouseOver?.Controller?.HandleMouseEntered();
            }
        }

        private void HandleKeyPressed(object? sender, KeyEventArgs e)
        {
            _focus?.Controller?.HandleKeyPressed(e);
        }

        private void HandleTextEntered(object? sender, TextEventArgs e)
        {
            _focus?.Controller?.HandleTextEntered(e);
        }

        private void HandleMouseButtonClicked(object? sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                _focus = _mouseOver;
            }
            // Translate into component relative coordinates.
            _mouseOver?.Controller?.HandleMouseButtonClicked(e);
        }

        private void HandleMouseWheelScrolled(object? sender, MouseWheelScrollEventArgs e)
        {
            _mouseOver?.Controller?.HandleMouseWheelScrolled(e);
        }
    }
}
