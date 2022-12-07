using Cardamom.Graphics.Ui.Elements;
using OpenTK.Windowing.Common;

namespace Cardamom.Graphics.Ui.Controller
{
    public class TableController : ClassedUiElementController<UiSerialContainer>, IUiCollectionController
    {
        public EventHandler<ElementEventArgs>? ElementClicked { get; set; }

        public override void Bind(object @object)
        {
            base.Bind(@object);
            foreach (var element in _element!)
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

        private void HandleElementClicked(object? sender, MouseButtonEventArgs e)
        {
            ElementClicked?.Invoke(this, new ElementEventArgs((IController)sender!, e));
        }
    }
}
