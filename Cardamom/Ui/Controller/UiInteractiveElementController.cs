using Cardamom.Window;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Ui.Controller
{
    public class UiInteractiveElementController<T> : IController where T : IUiInteractiveElement
    {
        public EventHandler<MouseButtonEventArgs>? LeftClicked { get; set; }
        public EventHandler<MouseButtonEventArgs>? RightClicked { get; set; }

        protected T? _element;

        public void Bind(object @object)
        {
            _element = (T)@object;
        }

        public void Unbind()
        {
            _element = default;
        }

        public virtual void HandleKeyPressed(KeyboardKeyEventArgs e) { }

        public virtual void HandleMouseEntered() { }

        public virtual void HandleMouseLeft() { }

        public virtual void HandleMouseButtonClicked(MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:
                    LeftClicked?.Invoke(this, e);
                    break;
                case MouseButton.Right:
                    RightClicked?.Invoke(this, e);
                    break;
                default:
                    break;
            }
        }

        public virtual void HandleMouseButtonDragged(MouseButtonDragEventArgs e) { }

        public virtual void HandleMouseWheelScrolled(MouseWheelEventArgs e) { }

        public virtual void HandleFocusEntered() { }

        public virtual void HandleFocusLeft() { }
    }
}
