using Cardamom.Graphics.Ui.Elements;

namespace Cardamom.Graphics.Ui.Controller
{
    public class TableController : ClassedUiElementController<UiSerialContainer>
    {
        public override void HandleMouseEntered()
        {
            SetHover(true);
        }

        public override void HandleMouseLeft()
        {
            SetHover(false);
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
