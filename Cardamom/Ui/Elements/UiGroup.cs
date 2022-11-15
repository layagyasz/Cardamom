using SFML.Graphics;

namespace Cardamom.Ui.Elements
{
    public class UiGroup : IEnumerable<IUiElement>, IRenderable
    {
        private readonly List<IUiElement> _elements = new();

        public void Add(IUiElement element)
        {
            _elements.Add(element);
        }

        public IEnumerator<IUiElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Draw(RenderTarget target, Transform transform)
        {
            foreach (var element in _elements)
            {
                if (element.Visible)
                {
                    element.Draw(target, transform);
                }
            }
        }

        public void Update(UiContext context, Transform transform, long delta)
        {
            _elements.ForEach(x => x.Update(context, transform, delta));
        }
    }
}
