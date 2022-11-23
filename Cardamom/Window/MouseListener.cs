using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Window
{
    public class MouseListener
    {
        public EventHandler<MouseButtonEventArgs>? MouseButtonClicked { get; set; }
        public EventHandler<MouseButtonDragEventArgs>? MouseButtonDragged { get; set; }
        public EventHandler<MouseWheelEventArgs>? MouseWheelScrolled { get; set; }

        private RenderWindow? _window;

        private MouseButton? _depressedButton;
        private Vector2 _depressedPosition;
        private bool _drag;
        private Vector2 _draggedPosition;

        public void Bind(RenderWindow window)
        {
            _window = window;
            window.MouseButtonPressed += HandleMouseButtonPressed;
            window.MouseButtonReleased += HandleMouseButtonReleased;
            window.MouseWheelScrolled += HandleMouseWheelScrolled;
            window.MouseMoved += HandleMouseMoved;
        }

        public Vector2 GetMousePosition()
        {
            return _window!.GetMousePosition();
        }

        private void HandleMouseButtonPressed(object? sender, MouseButtonEventArgs e)
        {
            if (_depressedButton == null)
            {
                _depressedButton = e.Button;
                _depressedPosition = GetMousePosition();
                _draggedPosition = _depressedPosition;
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
                _draggedPosition = e.Position;
                MouseButtonDragged?.Invoke(
                    this,
                    new(
                        (MouseButton)_depressedButton!,
                        _depressedPosition,
                        _draggedPosition,
                        e.Delta));
            }
        }

        private void HandleMouseWheelScrolled(object? sender, MouseWheelEventArgs e)
        {
            MouseWheelScrolled?.Invoke(this, e);
        }
    }
}
