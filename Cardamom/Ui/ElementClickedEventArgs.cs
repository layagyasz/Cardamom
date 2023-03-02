using Cardamom.Ui.Controller.Element;

namespace Cardamom.Ui
{
    public readonly struct ElementClickedEventArgs
    {
        public IElementController Element { get; }
        public MouseButtonClickEventArgs MouseEvent { get; }

        public ElementClickedEventArgs(IElementController element, MouseButtonClickEventArgs mouseEvent)
        {
            Element = element;
            MouseEvent = mouseEvent;
        }

        public override string ToString()
        {
            return string.Format($"[ElementEventArgs: Element={Element}, MouseEvent={MouseEvent}");
        }
    }
}
