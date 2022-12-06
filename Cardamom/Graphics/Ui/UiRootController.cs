using Cardamom.Graphics.Ui.Controller;
using Cardamom.Window;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Graphics.Ui
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
            var newMouseOver = _context!.GetTopElement();
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

        private void HandleKeyPressed(object? sender, KeyboardKeyEventArgs e)
        {
            _focus?.Controller?.HandleKeyPressed(e);
        }

        private void HandleMouseButtonClicked(object? sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
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
            Consume(_mouseOver, x => x.Controller?.HandleMouseButtonDragged(e) ?? false);
        }

        private void HandleMouseWheelScrolled(object? sender, MouseWheelEventArgs e)
        {
            Consume(_mouseOver, x => x.Controller?.HandleMouseWheelScrolled(e) ?? false);
        }

        private static void Consume(IControlled? root, Func<IControlled, bool> consumer)
        {
            while (root != null && !consumer(root))
            {
                root = root.Parent;
            }
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
