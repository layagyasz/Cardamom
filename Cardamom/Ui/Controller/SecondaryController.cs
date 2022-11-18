using SFML.Window;

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

        public void HandleKeyPressed(KeyEventArgs e) { }

        public void HandleTextEntered(TextEventArgs e) { }

        public void HandleMouseEntered() { }

        public void HandleMouseLeft() { }

        public void HandleMouseButtonClicked(MouseButtonEventArgs e) { }

        public void HandleMouseButtonDragged(MouseButtonDragEventArgs e) { }

        public void HandleMouseWheelScrolled(MouseWheelScrollEventArgs e) { }

        public void HandleFocusEntered() { }

        public void HandleFocusLeft() { }
    }
}
