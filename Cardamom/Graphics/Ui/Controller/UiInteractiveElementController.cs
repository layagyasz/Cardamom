using Cardamom.Window;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Graphics.Ui.Controller
{
    public class UiInteractiveElementController<T> : IController where T : IUiInteractiveElement
    {
        public EventHandler<MouseButtonEventArgs>? Clicked { get; set; }
        public EventHandler<EventArgs>? Focused { get; set; }

        protected T? _element;

        public void Bind(object @object)
        {
            _element = (T)@object;
        }

        public void Unbind()
        {
            _element = default;
        }

        public T GetElement()
        {
            return _element!;
        }

        public virtual void HandleKeyPressed(KeyboardKeyEventArgs e) { }

        public virtual void HandleMouseEntered() { }

        public virtual void HandleMouseLeft() { }

        public virtual void HandleMouseButtonClicked(MouseButtonEventArgs e)
        {
            Clicked?.Invoke(this, e);
        }

        public virtual void HandleMouseButtonDragged(MouseButtonDragEventArgs e) { }

        public virtual void HandleMouseWheelScrolled(MouseWheelEventArgs e) { }

        public virtual void HandleFocusEntered()
        {
            Focused?.Invoke(this, EventArgs.Empty);
        }

        public virtual void HandleFocusLeft() { }
    }
}
