using Cardamom.Window;
using OpenTK.Windowing.Common;

namespace Cardamom.Ui.Controller
{
    public class SecondaryController<T> : IController
    {

        protected T? _element;

        public virtual void Bind(object @object)
        {
            _element = (T)@object;
        }

        public virtual void Unbind()
        {
            _element = default;
        }

        public void HandleKeyPressed(KeyboardKeyEventArgs e) { }

        public void HandleMouseEntered() { }

        public void HandleMouseLeft() { }

        public void HandleMouseButtonClicked(MouseButtonEventArgs e) { }

        public void HandleMouseButtonDragged(MouseButtonDragEventArgs e) { }

        public void HandleMouseWheelScrolled(MouseWheelEventArgs e) { }

        public void HandleFocusEntered() { }

        public void HandleFocusLeft() { }
    }
}
