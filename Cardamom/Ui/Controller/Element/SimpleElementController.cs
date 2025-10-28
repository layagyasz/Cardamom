using Cardamom.Audio;

namespace Cardamom.Ui.Controller.Element
{
    public class SimpleElementController : ClassedUiElementController<ClassedUiElement>
    {
        public SimpleElementController(AudioPlayer? audioPlayer)
            : base(audioPlayer) { }

        public override bool HandleMouseButtonClicked(MouseButtonClickEventArgs e)
        {
            Click();
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
    }
}
