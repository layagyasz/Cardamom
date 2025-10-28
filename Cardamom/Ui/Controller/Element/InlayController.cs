using Cardamom.Audio;

namespace Cardamom.Ui.Controller.Element
{
    public class InlayController : SimpleElementController
    {
        public InlayController(AudioPlayer? audioPlayer)
            : base(audioPlayer) { }

        public override bool HandleMouseButtonClicked(MouseButtonClickEventArgs e)
        {
            Clicked?.Invoke(this, e);
            Click();
            return false;
        }
    }
}
