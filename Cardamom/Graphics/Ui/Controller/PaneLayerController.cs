using Cardamom.Graphics.Ui.Elements;

namespace Cardamom.Graphics.Ui.Controller
{
    public class PaneLayerController : NoOpController<UiGroupLayer>
    {
        public override void Bind(object @object)
        {
            base.Bind(@object);
            foreach (var pane in _element!)
            {
                if (pane is IUiElement element)
                {
                    if (element.Controller is PaneController controller)
                    {
                        controller.Closed += HandleClose;
                        controller.Focused += HandleFocus;
                    }
                }
            }
        }

        public void Add(IUiElement pane)
        {
            if (pane.Controller is PaneController controller)
            {
                controller.Closed += HandleClose;
                controller.Focused += HandleFocus;
            }
            GetElement().Add(pane);
        }

        public void Remove(IUiElement pane)
        {
            if (pane.Controller is PaneController controller)
            {
                controller.Closed -= HandleClose;
                controller.Focused -= HandleFocus;
            }
            GetElement().Remove(pane);
        }

        private void HandleFocus(object? sender, EventArgs e)
        {
            if (sender is PaneController controller)
            {
                GetElement().Remove(controller.GetElement());
                GetElement().Add(controller.GetElement());
            }
        }

        private void HandleClose(object? sender, EventArgs e)
        {
            if (sender is PaneController controller)
            {
                Remove(controller.GetElement());
            }
        }
    }
}
