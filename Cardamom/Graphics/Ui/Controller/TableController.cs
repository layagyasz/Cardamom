using Cardamom.Graphics.Ui.Elements;

namespace Cardamom.Graphics.Ui.Controller
{
    public class TableController : ClassedUiElementController<UiSerialContainer>
    {
        public override bool HandleMouseEntered()
        {
            SetHover(true);
            return true;
        }

        public override bool HandleMouseLeft()
        {
            SetHover(false);
            return true;
        }

        public override bool HandleFocusEntered()
        {
            SetFocus(true);
            return true;
        }

        public override bool HandleFocusLeft()
        {
            SetFocus(false);
            return true;
        }
    }
}
