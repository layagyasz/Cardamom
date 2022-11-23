using Cardamom.Ui.Elements;
using System.Diagnostics.Tracing;

namespace Cardamom.Ui.Controller
{
    public class PaneLayerController : SecondaryController<UiLayer>
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

        public override void Unbind()
        {
            base.Unbind();
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
