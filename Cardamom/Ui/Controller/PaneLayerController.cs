using Cardamom.Ui.Controller.Element;

namespace Cardamom.Ui.Controller
{
    public class PaneLayerController : IController
    {
        private UiGroupLayer? _panes;

        public void Bind(object @object)
        {
            _panes = (UiGroupLayer)@object;
            foreach (var pane in _panes)
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

        public void Unbind()
        {
            foreach (var pane in _panes!)
            {
                if (pane is IUiElement element)
                {
                    if (element.Controller is PaneController controller)
                    {
                        controller.Closed -= HandleClose;
                        controller.Focused -= HandleFocus;
                    }
                }
            }
            _panes = null;
        }

        public void Add(IUiElement pane)
        {
            if (pane.Controller is PaneController controller)
            {
                controller.Closed += HandleClose;
                controller.Focused += HandleFocus;
            }
            _panes!.Add(pane);
        }

        public void Clear()
        {
            foreach (var element in _panes!)
            {
                if (element is IControlledElement pane)
                {
                    if (pane.Controller is PaneController controller)
                    {
                        controller.Closed -= HandleClose;
                        controller.Focused -= HandleFocus;
                    }
                }
            }
            _panes!.Clear();
        }

        public void Remove(IUiElement pane)
        {
            if (pane.Controller is PaneController controller)
            {
                controller.Closed -= HandleClose;
                controller.Focused -= HandleFocus;
            }
            _panes!.Remove(pane);
        }

        private void HandleFocus(object? sender, EventArgs e)
        {
            if (sender is PaneController controller)
            {
                _panes!.Remove(controller.GetElement());
                _panes!.Add(controller.GetElement());
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
