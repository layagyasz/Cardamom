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
            _panes.ElementRemoved += HandleElementRemoved;
            foreach (var pane in _panes)
            {
                if (pane is IUiElement element)
                {
                    if (element.Controller is IPaneController controller)
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
            _panes.ElementRemoved -= HandleElementRemoved;
            foreach (var pane in _panes!)
            {
                if (pane is IUiElement element)
                {
                    if (element.Controller is IPaneController controller)
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
                    if (pane.Controller is IPaneController controller)
                    {
                        controller.Closed -= HandleClose;
                        controller.Focused -= HandleFocus;
                    }
                }
            }
            _panes!.Clear();
        }

        private void BindElement(IUiElement pane)
        {
            if (pane.Controller is IPaneController controller)
            {
                controller.Closed += HandleClose;
                controller.Focused += HandleFocus;
            }
        }

        private void UnbindElement(IUiElement pane)
        {
            if (pane.Controller is IPaneController controller)
            {
                controller.Closed -= HandleClose;
                controller.Focused -= HandleFocus;
            }
        }

        private void HandleElementAdded(object? sender, ElementEventArgs e)
        {
            BindElement((IUiElement)e.Element);
        }

        private void HandleElementRemoved(object? sender, ElementEventArgs e)
        {
            UnbindElement((IUiElement)e.Element);
        }

        private void HandleFocus(object? sender, EventArgs e)
        {
            if (sender is IPaneController controller)
            {
                _panes!.Remove(controller.GetPane());
                _panes!.Add(controller.GetPane());
            }
        }

        private void HandleClose(object? sender, EventArgs e)
        {
            if (sender is PaneController controller)
            {
                UnbindElement(controller.GetElement());
                _panes!.Remove(controller.GetElement());
            }
        }
    }
}
