using Cardamom.Ui.Controller.Element;
using OpenTK.Windowing.Common;

namespace Cardamom.Ui
{
    public readonly struct ElementEventArgs
    {
        public IElementController Element { get; }
        public MouseButtonClickEventArgs MouseEvent { get; }

        public ElementEventArgs(IElementController element, MouseButtonClickEventArgs mouseEvent)
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
