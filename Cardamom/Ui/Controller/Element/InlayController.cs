namespace Cardamom.Ui.Controller.Element
{
    public class InlayController : ClassedUiElementController<ClassedUiElement>
    {
        public override bool HandleMouseButtonClicked(MouseButtonClickEventArgs e)
        {
            Clicked?.Invoke(this, e);
            return false;
        }

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
    }
}
