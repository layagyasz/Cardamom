using Cardamom.Ui.Controller.Element;
using Cardamom.Window;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Ui
{
    public class UiRootController
    {
        private IUiContext? _context;

        private IControlledElement? _focus;
        private HashSet<IControlledElement> _focusAncestry = new();

        private IControlledElement? _mouseOver;
        private HashSet<IControlledElement> _mouseOverAncestry = new();

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

        public void Bind(IUiContext context)
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

        public void SetFocus(IControlledElement? element)
        {
            if (element != _focus)
            {
                var newAncestry = GetAncestry(element);
                bool anyNewFocus = false;
                foreach (var e in newAncestry.Except(_focusAncestry))
                {
                    anyNewFocus |= e.Controller.HandleFocusEntered();
                }
                if (anyNewFocus)
                {
                    foreach (var e in _focusAncestry.Except(newAncestry))
                    {
                        e.Controller?.HandleFocusLeft();
                    }
                    _focus = element;
                    _focusAncestry = newAncestry;
                }
            }
        }

        private void HandleKeyDown(object? sender, KeyDownEventArgs e)
        {
            Consume(_mouseOver, x => x.Controller?.HandleKeyDown(e) ?? false);
        }

        private void HandleTextEntered(object? sender, TextEnteredEventArgs e)
        {
            Consume(_focus, x => x?.Controller?.HandleTextEntered(e) ?? false);
        }

        private void HandleMouseButtonClicked(object? sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                SetFocus(_context?.GetTopElement());
            }
            MouseButtonClickEventArgs mouseEvent =
                new()
                {
                    Action = e.Action,
                    Button = e.Button,
                    Modifiers = e.Modifiers,
                    IsPressed = e.IsPressed,
                    Position = _context!.GetTopIntersection()
                };
            Consume(_mouseOver, x => x.Controller?.HandleMouseButtonClicked(mouseEvent) ?? false);
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

        private static void Consume(IControlledElement? root, Func<IControlledElement, bool> consumer)
        {
            while (root != null && !consumer(root))
            {
                root = root.Parent;
            }
        }

        private static HashSet<IControlledElement> GetAncestry(IControlledElement? element)
        {
            var result = new HashSet<IControlledElement>();
            while (element != null)
            {
                result.Add(element);
                element = element.Parent;
            }
            return result;
        }
    }
}
