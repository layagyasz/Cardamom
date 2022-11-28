using Cardamom.Window;
using OpenTK.Windowing.Common;

namespace Cardamom.Graphics.Ui.Controller
{
    public interface IController
    {
        void Bind(object @object);
        void Unbind();
        void HandleKeyPressed(KeyboardKeyEventArgs e);
        void HandleMouseEntered();
        void HandleMouseLeft();
        void HandleMouseButtonClicked(MouseButtonEventArgs e);
        void HandleMouseButtonDragged(MouseButtonDragEventArgs e);
        void HandleMouseWheelScrolled(MouseWheelEventArgs e);
        void HandleFocusEntered();
        void HandleFocusLeft();
    }
}
