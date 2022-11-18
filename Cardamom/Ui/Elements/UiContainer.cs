using Cardamom.Ui.Controller;
using SFML.Graphics;

namespace Cardamom.Ui.Elements
{
    public class UiContainer : SimpleUiElement, IEnumerable<IUiElement>
    {
        private readonly List<IUiElement> _elements = new();

        public UiContainer(Class @class, IController controller)
            : base(@class, controller) { }

        public void Add(IUiElement element)
        {
            _elements.Add(element);
            element.Parent = this;
        }

        public IEnumerator<IUiElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Draw(RenderTarget target, Transform transform)
        {
            base.Draw(target, transform);
            transform.Translate(Position + LeftMargin + LeftPadding);
            foreach (var element in _elements)
            {
                element.Draw(target, transform);
            }
        }

        public override void Update(UiContext context, Transform transform, long delta)
        {
            base.Update(context, transform, delta);
            transform.Translate(Position + LeftMargin + LeftPadding);
            _elements.ForEach(x => x.Update(context, transform, delta));
        }
    }
}
