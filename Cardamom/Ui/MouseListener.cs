using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Cardamom.Ui
{
    public class MouseListener
    {
        public EventHandler<MouseButtonEventArgs>? MouseButtonClicked { get; set; }
        public EventHandler<MouseButtonDragEventArgs>? MouseButtonDragged { get; set; }
        public EventHandler<MouseWheelScrollEventArgs>? MouseWheelScrolled { get; set; }

        private RenderWindow? _window;

        private Mouse.Button? _depressedButton;
        private Vector2f _depressedPosition;
        private bool _drag;
        private Vector2f _draggedPosition;

        public void Bind(RenderWindow window)
        {
            _window = window;
            window.MouseButtonPressed += HandleMouseButtonPressed;
            window.MouseButtonReleased += HandleMouseButtonReleased;
            window.MouseWheelScrolled += HandleMouseWheelScrolled;
            window.MouseMoved += HandleMouseMoved;
        }

        public Vector2f GetMousePosition()
        {
            var position = Mouse.GetPosition(_window);
            return new Vector2f(position.X, position.Y);
        }

        private void HandleMouseButtonPressed(object? sender, MouseButtonEventArgs e)
        {
            if (_depressedButton != null)
            {
                _depressedButton = e.Button;
                _depressedPosition = GetMousePosition();
            }
        }

        private void HandleMouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (_depressedButton == e.Button)
            {
                _depressedButton = null;
                _depressedPosition = default;
                _draggedPosition = default;
                if (!_drag)
                {
                    MouseButtonClicked?.Invoke(this, e);
                }
                _drag = false;
            }
        }

        private void HandleMouseMoved(object? sender, MouseMoveEventArgs e)
        {
            if (_drag || _depressedButton != null)
            {
                _drag = true;
                var newPosition = new Vector2f(e.X, e.Y);
                var delta = newPosition - _draggedPosition;
                _draggedPosition = newPosition;
                MouseButtonDragged?.Invoke(
                    this, 
                    new(
                        (Mouse.Button)Precondition.NotNull(_depressedButton),
                        _depressedPosition, 
                        _draggedPosition, 
                        delta));
;            }
        }

        private void HandleMouseWheelScrolled(object? sender, MouseWheelScrollEventArgs e)
        {
            MouseWheelScrolled?.Invoke(this, e);
        }
    }
}
