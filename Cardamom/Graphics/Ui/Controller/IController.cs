using Cardamom.Window;
using OpenTK.Windowing.Common;

namespace Cardamom.Graphics.Ui.Controller
{
    public interface IController
    {
        void Bind(object @object);
        void Unbind();
        bool HandleKeyPressed(KeyboardKeyEventArgs e);
        bool HandleMouseEntered();
        bool HandleMouseLeft();
        bool HandleMouseButtonClicked(MouseButtonEventArgs e);
        bool HandleMouseButtonDragged(MouseButtonDragEventArgs e);
        bool HandleMouseWheelScrolled(MouseWheelEventArgs e);
        bool HandleFocusEntered();
        bool HandleFocusLeft();
    }
}
