using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Window
{
    public class MouseListener
    {
        private static readonly long s_LingerTime = 1000;

        public EventHandler<MouseButtonEventArgs>? MouseButtonClicked { get; set; }
        public EventHandler<MouseButtonDragEventArgs>? MouseButtonDragged { get; set; }
        public EventHandler<MouseWheelEventArgs>? MouseWheelScrolled { get; set; }
        public EventHandler<EventArgs>? MouseLingered { get; set; }
        public EventHandler<EventArgs>? MouseLingerBroken { get; set; }

        private RenderWindow? _window;

        // Variables to track mouse dragging
        private MouseButton? _depressedButton;
        private Vector2 _depressedPosition;
        private bool _drag;
        private Vector2 _draggedPosition;

        // Variables to track mouse lingering
        private long _lingerTime;

        public void Bind(RenderWindow window)
        {
            _window = window;
            window.MouseButtonPressed += HandleMouseButtonPressed;
            window.MouseButtonReleased += HandleMouseButtonReleased;
            window.MouseWheelScrolled += HandleMouseWheelScrolled;
            window.MouseMoved += HandleMouseMoved;
        }

        public void DispatchEvents(long delta)
        {
            if (_lingerTime < s_LingerTime && _lingerTime + delta >= s_LingerTime)
            {
                MouseLingered?.Invoke(this, EventArgs.Empty);
            }
            _lingerTime += delta;
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
            if (_lingerTime >= s_LingerTime)
            {
                MouseLingerBroken?.Invoke(this, EventArgs.Empty);
            }
            _lingerTime = 0;

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
                        e.Delta, 
                        WindowToNdc(e.Delta)));
            }
        }

        private void HandleMouseWheelScrolled(object? sender, MouseWheelEventArgs e)
        {
            MouseWheelScrolled?.Invoke(this, e);
        }

        private Vector2 WindowToNdc(Vector2 position)
        {
            var v = position / _window!.GetViewPort().Size;
            v.Y = -v.Y;
            return v;
        }
    }
}
