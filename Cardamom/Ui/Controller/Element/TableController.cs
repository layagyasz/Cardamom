using Cardamom.Audio;
using Cardamom.Ui.Elements;
using OpenTK.Windowing.Common;

namespace Cardamom.Ui.Controller.Element
{
    public class TableController : ClassedUiElementController<UiSerialContainer>
    {
        public float ScrollSpeed { get; set; }

        public TableController(AudioPlayer? audioPlayer, float scrollSpeed)
            : base(audioPlayer)
        {
            ScrollSpeed = scrollSpeed;
        }

        public void ResetOffset()
        {
            _element!.SetOffset(0);
        }

        public override bool HandleMouseButtonClicked(MouseButtonClickEventArgs e)
        {
            Click();
            Clicked?.Invoke(this, e);
            return true;
        }

        public override bool HandleMouseWheelScrolled(MouseWheelEventArgs e)
        {
            if (ScrollSpeed > float.Epsilon)
            {
                return _element!.TryAdjustOffset(ScrollSpeed * e.OffsetY);
            }
            return false;
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
