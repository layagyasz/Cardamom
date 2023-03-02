using Cardamom.Ui.Controller.Element;
using Cardamom.Ui.Elements;

namespace Cardamom.Ui.Controller
{
    public class PaneLayerController : IController
    {
        private UiGroup? _panes;

        public void Bind(object @object)
        {
            _panes = (UiGroup)@object;
            _panes.ElementAdded += HandleElementAdded;
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
            _panes!.ElementAdded -= HandleElementAdded;
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

        private void Add(IUiElement pane)
        {
            if (pane.Controller is PaneController controller)
            {
                controller.Closed += HandleClose;
                controller.Focused += HandleFocus;
            }
        }

        private void Remove(IUiElement pane)
        {
            if (pane.Controller is PaneController controller)
            {
                controller.Closed -= HandleClose;
                controller.Focused -= HandleFocus;
            }
        }

        private void HandleElementAdded(object? sender, ElementEventArgs e)
        {
            Add((IUiElement)e.Element);
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
                _panes!.Remove(controller.GetElement());
            }
        }
    }
}
