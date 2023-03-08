using Cardamom.Graphics;

namespace Cardamom.Ui
{
    public struct ElementEventArgs
    {
        public IRenderable Element { get; }

        public ElementEventArgs(IRenderable element)
        {
            Element = element;
        }
    }
}
