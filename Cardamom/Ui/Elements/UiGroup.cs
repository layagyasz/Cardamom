using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class UiGroup : IEnumerable<IRenderable>, IRenderable
    {
        private readonly List<IRenderable> _elements = new();

        public virtual void Initialize()
        {
            _elements.ForEach(x => x.Initialize());
        }

        public void ResizeContext(Vector3 bounds) { }

        public void Add(IRenderable element)
        {
            _elements.Add(element);
        }

        public void Remove(IRenderable element)
        {
            _elements.Remove(element);
        }

        public IEnumerator<IRenderable> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Draw(RenderTarget target, UiContext context)
        {
            foreach (var element in _elements)
            {
                element.Draw(target, context);
            }
        }

        public virtual void Update(long delta)
        {
            _elements.ForEach(x => x.Update(delta));
        }
    }
}
