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

        public override void HandleMouseWheelScrolled(MouseWheelEventArgs e)
        {
            _element!.TryAdjustOffset(ScrollSpeed * e.OffsetY);
        }
    }
}
