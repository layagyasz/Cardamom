using OpenTK.Windowing.Common;

namespace Cardamom.Graphics.Ui.Controller
{
    public class ScrollingTableController : TableController
    {
        public float ScrollSpeed { get; set; }

        public ScrollingTableController(float scrollSpeed)
        {
            ScrollSpeed = scrollSpeed;
        }

        public override bool HandleMouseWheelScrolled(MouseWheelEventArgs e)
        {
            _element!.TryAdjustOffset(ScrollSpeed * e.OffsetY);
            return true;
        }
    }
}
