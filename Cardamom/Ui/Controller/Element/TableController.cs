using Cardamom.Ui.Elements;

namespace Cardamom.Ui.Controller.Element
{
    public class TableController : ClassedUiElementController<UiSerialContainer>, IUiCollectionController
    {
        public EventHandler<ElementEventArgs>? ElementClicked { get; set; }

        public override void Bind(object @object)
        {
            base.Bind(@object);
            foreach (var element in _element!.Cast<IUiElement>())
            {
                element.Controller.Clicked += HandleElementClicked;
            }
        }

        public void Add(IUiElement element)
        {
            _element!.Add(element);
            element.Controller.Clicked += HandleElementClicked;
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
            ElementClicked?.Invoke(this, new ElementEventArgs((IElementController)sender!, e));
        }
    }
}
