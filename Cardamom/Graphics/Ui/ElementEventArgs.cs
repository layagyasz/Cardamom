using Cardamom.Graphics.Ui.Controller;
using OpenTK.Windowing.Common;

namespace Cardamom.Graphics.Ui
{
    public readonly struct ElementEventArgs
    {
        public IController Element { get; }
        public MouseButtonEventArgs MouseEvent { get; }

        public ElementEventArgs(IController element, MouseButtonEventArgs mouseEvent)
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
