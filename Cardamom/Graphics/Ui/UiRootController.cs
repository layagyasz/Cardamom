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

        public void Bind(KeyboardListener keyboardListener)
        {
            keyboardListener.TextEntered += HandleTextEntered;
            keyboardListener.KeyDown += HandleKeyDown;
        }

        public void Bind(MouseListener mouseListener)
        {
            mouseListener.MouseButtonClicked += HandleMouseButtonClicked;
            mouseListener.MouseButtonDragged += HandleMouseButtonDragged;
            mouseListener.MouseWheelScrolled += HandleMouseWheelScrolled;
            mouseListener.MouseLingered += HandleMouseLingered;
            mouseListener.MouseLingerBroken += HandleMouseLingerBroken;
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

        private void HandleKeyDown(object? sender, KeyDownEventArgs e)
        {
            Consume(_mouseOver, x => x.Controller?.HandleKeyDown(e) ?? false);
        }

        private void HandleTextEntered(object? sender, TextEnteredEventArgs e)
        {
            _focus?.Controller?.HandleTextEntered(e);
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
            _mouseOver?.Controller?.HandleMouseButtonClicked(
                new() 
                { 
                    Action = e.Action,
                    Button = e.Button,
                    Modifiers = e.Modifiers,
                    IsPressed = e.IsPressed, 
                    Position = _context!.GetTopIntersection()
                });
        }

        private void HandleMouseButtonDragged(object? sender, MouseButtonDragEventArgs e)
        {
            Consume(_mouseOver, x => x.Controller?.HandleMouseButtonDragged(e) ?? false);
        }

        private void HandleMouseWheelScrolled(object? sender, MouseWheelEventArgs e)
        {
            Consume(_mouseOver, x => x.Controller?.HandleMouseWheelScrolled(e) ?? false);
        }

        private void HandleMouseLingered(object? sender, EventArgs e)
        {
            Consume(_mouseOver, x => x.Controller?.HandleMouseLingered() ?? false);
        }

        private void HandleMouseLingerBroken(object? sender, EventArgs e)
        {
            Consume(_mouseOver, x => x.Controller?.HandleMouseLingerBroken() ?? false);
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
