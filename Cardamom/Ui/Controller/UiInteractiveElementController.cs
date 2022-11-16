using SFML.Window;

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

        public virtual void HandleKeyPressed(KeyEventArgs e) { }

        public virtual void HandleTextEntered(TextEventArgs e) { }

        public virtual void HandleMouseEntered() { }

        public virtual void HandleMouseLeft() { }

        public virtual void HandleMouseButtonClicked(MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case Mouse.Button.Left:
                    LeftClicked?.Invoke(this, e);
                    break;
                case Mouse.Button.Right:
                    RightClicked?.Invoke(this, e);
                    break;
                default:
                    break;
            }
        }

        public virtual void HandleMouseButtonDragged(MouseButtonDragEventArgs e) { }

        public virtual void HandleMouseWheelScrolled(MouseWheelScrollEventArgs e) { }

        public virtual void HandleFocusEntered() { }

        public virtual void HandleFocusLeft() { }
    }
}
