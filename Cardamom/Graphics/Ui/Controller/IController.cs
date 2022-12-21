using Cardamom.Window;
using OpenTK.Windowing.Common;

namespace Cardamom.Graphics.Ui.Controller
{
    public interface IController
    {
        EventHandler<MouseButtonEventArgs>? Clicked { get; set; }
        EventHandler<EventArgs>? Focused { get; set; }

        void Bind(object @object);
        void Unbind();
        bool HandleKeyDown(KeyDownEventArgs e);
        bool HandleTextEntered(TextEnteredEventArgs e);
        bool HandleMouseEntered();
        bool HandleMouseLeft();
        bool HandleMouseButtonClicked(MouseButtonEventArgs e);
        bool HandleMouseButtonDragged(MouseButtonDragEventArgs e);
        bool HandleMouseWheelScrolled(MouseWheelEventArgs e);
        bool HandleFocusEntered();
        bool HandleFocusLeft();
    }
}
