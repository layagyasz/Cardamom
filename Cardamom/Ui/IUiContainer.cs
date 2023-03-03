using Cardamom.Graphics;

namespace Cardamom.Ui
{
    public interface IUiContainer : IEnumerable<IRenderable>, IUiElement, IDisposable
    { 
        EventHandler<ElementEventArgs>? ElementAdded { get; set; }
        EventHandler<ElementEventArgs>? ElementRemoved { get; set; }

        void Add(IUiElement element);
        void Remove(IUiElement element);
    }
}
