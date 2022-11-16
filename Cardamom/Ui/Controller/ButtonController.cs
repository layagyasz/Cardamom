namespace Cardamom.Ui.Controller
{
    public class ButtonController : ClassedUiElementController<ClassedUiElement>
    {
        public override void HandleMouseEntered()
        {
            SetHover(true);
        }

        public override void HandleMouseLeft()
        {
            SetHover(false);
        }
    }
}
