using Cardamom.Ui.Elements;

namespace Cardamom.Ui.Controller
{
    public abstract class DynamicComponentControllerBase : IController
    {
        protected UiCompoundComponent? _component;

        public virtual void Bind(object @object)
        {
            _component = (UiCompoundComponent)@object;
            _component.ElementAdded += HandleElementAdded;
            _component.ElementRemoved += HandleElementRemoved;
            foreach (var element in _component)
            {
                BindElement(element);
            }
        }

        public abstract void BindElement(IUiElement element);

        public virtual void Unbind()
        {
            foreach (var element in _component!)
            {
                UnbindElement(element);
            }
            _component.ElementAdded -= HandleElementAdded;
            _component.ElementRemoved -= HandleElementRemoved;
            _component = null;
        }

        public abstract void UnbindElement(IUiElement element);

        public void HandleElementAdded(object? sender, ElementEventArgs e)
        {
            BindElement((IUiElement)e.Element);
        }

        public void HandleElementRemoved(object? sender, ElementEventArgs e)
        {
            UnbindElement((IUiElement)e.Element);
        }
    }
}
