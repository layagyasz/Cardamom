using Cardamom.Window;
using OpenTK.Windowing.Common;

namespace Cardamom.Ui.Controller.Element
{
    public interface IElementController : IController
    {
        EventHandler<MouseButtonClickEventArgs>? Clicked { get; set; }
        EventHandler<EventArgs>? Focused { get; set; }
        EventHandler<EventArgs>? FocusLeft { get; set; }
        EventHandler<MouseButtonDragEventArgs>? MouseDragged { get; set; }
        EventHandler<EventArgs>? MouseEntered { get; set; }
        EventHandler<EventArgs>? MouseLeft { get; set; }

        bool HandleKeyDown(KeyDownEventArgs e);
        bool HandleTextEntered(TextEnteredEventArgs e);
        bool HandleMouseEntered();
        bool HandleMouseLeft();
        bool HandleMouseButtonClicked(MouseButtonClickEventArgs e);
        bool HandleMouseButtonDragged(MouseButtonDragEventArgs e);
        bool HandleMouseWheelScrolled(MouseWheelEventArgs e);
        bool HandleMouseLingered();
        bool HandleMouseLingerBroken();
        bool HandleFocusEntered();
        bool HandleFocusLeft();
    }
}
