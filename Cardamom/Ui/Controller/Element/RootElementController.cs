using Cardamom.Audio;

namespace Cardamom.Ui.Controller.Element
{
    public class RootElementController : SimpleElementController
    {
        public RootElementController(AudioPlayer? audioPlayer)
            : base(audioPlayer) { }

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
