namespace Cardamom.Ui.Controller.Element
{
    public class RootElementController : ClassedUiElementController<ClassedUiElement>
    {
        public override bool HandleMouseButtonClicked(MouseButtonClickEventArgs e)
        {
            Clicked?.Invoke(this, e);
            return true;
        }

        public override bool HandleMouseEntered()
        {
            SetHover(true);
            MouseEntered?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public override bool HandleMouseLeft()
        {
            SetHover(false);
            MouseLeft?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public override bool HandleFocusEntered()
        {
            SetFocus(true);
            Focused?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public override bool HandleFocusLeft()
        {
            SetFocus(false);
            FocusLeft?.Invoke(this, EventArgs.Empty);
            return true;
        }
    }
}
