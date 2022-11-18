using Cardamom.Ui.Controller;
using SFML.Graphics;
using SFML.Window;

namespace Cardamom.Ui
{
    public class UiRootController
    {
        private UiContext? _context;

        private IControlled? _focus;
        private HashSet<IControlled> _focusAncestry = new();

        private IControlled? _mouseOver;
        private HashSet<IControlled> _mouseOverAncestry = new();

        public void Bind(RenderWindow window)
        {
            window.KeyPressed += HandleKeyPressed;
            window.TextEntered += HandleTextEntered;
        }

        public void Bind(MouseListener mouseListener)
        {
            mouseListener.MouseButtonClicked += HandleMouseButtonClicked;
            mouseListener.MouseButtonDragged += HandleMouseButtonDragged;
            mouseListener.MouseWheelScrolled += HandleMouseWheelScrolled;
        }

        public void Bind(UiContext context)
        {
            _context = context;
        }

        public void DispatchEvents()
        {
            var newMouseOver = _context?.GetTopElement();
            if (newMouseOver != _mouseOver)
            {
                var newAncestry = GetAncestry(newMouseOver);
                foreach (var element in _mouseOverAncestry.Except(newAncestry))
                {
                    element.Controller?.HandleMouseLeft();
                }
                foreach (var element in newAncestry.Except(_mouseOverAncestry))
                {
                    element.Controller?.HandleMouseEntered();
                }
                _mouseOver = newMouseOver;
                _mouseOverAncestry = newAncestry;           
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
                var newFocus = _context?.GetTopElement();
                if (newFocus != _focus)
                {
                    var newAncestry = GetAncestry(newFocus);
                    foreach (var element in _focusAncestry.Except(newAncestry))
                    {
                        element.Controller?.HandleFocusLeft();
                    }
                    foreach (var element in newAncestry.Except(_focusAncestry))
                    {
                        element.Controller?.HandleFocusEntered();
                    }
                    _focus = newFocus;
                    _focusAncestry = newAncestry;
                }
            }
            // Translate into component relative coordinates.
            _mouseOver?.Controller?.HandleMouseButtonClicked(e);
        }

        private void HandleMouseButtonDragged(object? sender, MouseButtonDragEventArgs e)
        {
            _mouseOver?.Controller?.HandleMouseButtonDragged(e);
        }

        private void HandleMouseWheelScrolled(object? sender, MouseWheelScrollEventArgs e)
        {
            _mouseOver?.Controller?.HandleMouseWheelScrolled(e);
        }

        private static HashSet<IControlled> GetAncestry(IControlled? element)
        {
            var result = new HashSet<IControlled>();
            while (element != null)
            {
                result.Add(element);
                element = element.Parent;
            }
            return result;
        }
    }
}
