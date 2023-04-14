namespace Cardamom.Ui
{
    public interface IUiContainer : IEnumerable<IUiElement>, IUiElement, IDisposable
    { 
        EventHandler<ElementEventArgs>? ElementAdded { get; set; }
        EventHandler<ElementEventArgs>? ElementRemoved { get; set; }

        public int Count { get; }

        void Add(IUiElement element);
        void Clear(bool dispose);
        void Remove(IUiElement element);
    }
}
