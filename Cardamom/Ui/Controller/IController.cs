using SFML.Window;

namespace Cardamom.Ui.Controller
{
    public interface IController
    {
        void Bind(object @object);
        void Unbind();
        void HandleKeyPressed(KeyEventArgs e);
        void HandleTextEntered(TextEventArgs e);
        void HandleMouseEntered();
        void HandleMouseLeft();
        void HandleMouseButtonClicked(MouseButtonEventArgs e);
        void HandleMouseButtonDragged(MouseButtonDragEventArgs e);
        void HandleMouseWheelScrolled(MouseWheelScrollEventArgs e);
        void HandleFocusEntered();
        void HandleFocusLeft();
    }
}
