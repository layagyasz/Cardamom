using Cardamom.Window;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Graphics.Ui.Controller
{
    public class PaneController : ClassedUiElementController<ClassedUiElement>
    {
        public EventHandler<EventArgs>? Closed { get; set; }

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

        public override bool HandleMouseButtonDragged(MouseButtonDragEventArgs e)
        {
            if (e.Button == MouseButton.Left && _element != null)
            {
                _element.Position += new Vector3(e.Delta.X, e.Delta.Y, 0f);
            }
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
