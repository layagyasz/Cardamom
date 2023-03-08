using Cardamom.Ui.Elements;

namespace Cardamom.Ui.Controller.Element
{
    public class TableController : ClassedUiElementController<UiSerialContainer>, IUiCollectionController
    {
        public EventHandler<ElementClickedEventArgs>? ElementClicked { get; set; }

        public override void Bind(object @object)
        {
            base.Bind(@object);
            _element!.ElementAdded += HandleElementAdded;
            _element!.ElementRemoved += HandleElementRemoved;
            foreach (var element in _element!)
            {
                BindElement(element);
            }
        }

        public virtual void BindElement(IUiElement element)
        {
            element.Controller.Clicked += HandleElementClicked;
        }

        public override void Unbind()
        {
            _element!.ElementAdded += HandleElementAdded;
            _element!.ElementRemoved += HandleElementRemoved;
            foreach (var element in _element!)
            {
                UnbindElement(element);
            }
            base.Unbind();
        }

        public virtual void UnbindElement(IUiElement element)
        {
            element.Controller.Clicked -= HandleElementClicked;
        }

        public virtual void HandleElementAdded(object? sender, ElementEventArgs e)
        {
            BindElement((IUiElement)e.Element);
        }

        public virtual void HandleElementRemoved(object? sender, ElementEventArgs e)
        {
            UnbindElement((IUiElement)e.Element);
        }

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

        private void HandleElementClicked(object? sender, MouseButtonClickEventArgs e)
        {
            ElementClicked?.Invoke(this, new ElementClickedEventArgs((IElementController)sender!, e));
        }
    }
}
