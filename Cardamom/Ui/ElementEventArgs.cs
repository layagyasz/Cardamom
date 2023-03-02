namespace Cardamom.Ui
{
    public struct ElementEventArgs
    {
        public object Element { get; }

        public ElementEventArgs(object element)
        {
            Element = element;
        }
    }
}
