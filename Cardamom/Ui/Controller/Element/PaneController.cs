using Cardamom.Window;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Ui.Controller.Element
{
    public class PaneController : ClassedUiElementController<ClassedUiElement>, IPaneController
    {
        public EventHandler<EventArgs>? Closed { get; set; }

        public IUiElement GetPane()
        {
            return _element!;
        }

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

        public override bool HandleMouseButtonDragged(MouseButtonDragEventArgs e)
        {
            if (e.Button == MouseButton.Left && _element != null)
            {
                _element.Position += new Vector3(e.Delta.X, e.Delta.Y, 0f);
            }
            MouseDragged?.Invoke(this, e);
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
