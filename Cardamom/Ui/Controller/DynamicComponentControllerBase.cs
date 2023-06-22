using Cardamom.Ui.Elements;

namespace Cardamom.Ui.Controller
{
    public abstract class DynamicComponentControllerBase : IController
    {
        private readonly int _bindRecursionDepth;
        
        protected UiCompoundComponent? _component;

        protected DynamicComponentControllerBase(int bindRecursionDepth = 0)
        {
            _bindRecursionDepth = bindRecursionDepth;
        }

        public virtual void Bind(object @object)
        {
            _component = (UiCompoundComponent)@object;
            BindContainer(_component, /* depth= */ 0);
        }

        private void BindElement(IUiElement element, int depth)
        {
            if (depth <= _bindRecursionDepth && element is IUiContainer container)
            {
                BindContainer(container, depth + 1);
            }
            BindElement(element);
        }

        protected abstract void BindElement(IUiElement element);

        public virtual void Unbind()
        {
            UnbindContainer(_component!, /* depth= */ 0);
            _component = null;
        }


        private void UnbindElement(IUiElement element, int depth)
        {
            if (depth <= _bindRecursionDepth && element is IUiContainer container)
            {
                UnbindContainer(container, depth);
            }
            UnbindElement(element);
        }

        protected abstract void UnbindElement(IUiElement element);

        protected IEnumerable<IUiElement> GetChildren()
        {
            return GetChildren(_component!, /* depth= */ 0);
        }

        private void BindContainer(IUiContainer container, int depth)
        {
            container.ElementAdded += HandleElementAdded;
            container.ElementRemoved += HandleElementRemoved;
            foreach (var element in container)
            {
                BindElement(element, depth + 1);
            }
        }

        private void UnbindContainer(IUiContainer container, int depth)
        {
            container.ElementAdded -= HandleElementAdded;
            container.ElementRemoved -= HandleElementRemoved;
            foreach (var element in container)
            {
                UnbindElement(element, depth + 1);
            }
        }

        private IEnumerable<IUiElement> GetChildren(IUiElement root, int depth)
        {
            if (depth <= _bindRecursionDepth && root is IUiContainer container)
            {
                foreach (var child in container)
                {
                    foreach (var c in GetChildren(child, depth + 1))
                    {
                        yield return c;
                    }
                }
            }
            yield return root;
        }

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
