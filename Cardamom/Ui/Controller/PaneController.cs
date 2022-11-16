using SFML.Window;

namespace Cardamom.Ui.Controller
{
    public class PaneController : ClassedUiElementController<ClassedUiElement>
    {
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
            if (e.Button == Mouse.Button.Left && _element != null)
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
