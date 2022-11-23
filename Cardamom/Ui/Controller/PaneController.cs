using Cardamom.Window;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Ui.Controller
{
    public class PaneController : ClassedUiElementController<ClassedUiElement>
    {
        public EventHandler<EventArgs>? Closed { get; set; }

        public override void HandleMouseEntered()
        {
            SetHover(true);
        }

        public override void HandleMouseLeft()
        {
            SetHover(false);
        }

        public override void HandleMouseButtonDragged(MouseButtonDragEventArgs e)
        {
            if (e.Button == MouseButton.Left && _element != null)
            {
                _element.Position += e.Delta;
            }
        }

        public override void HandleFocusEntered()
        {
            SetFocus(true);
        }

        public override void HandleFocusLeft()
        {
            SetFocus(false);
        }
    }
}
