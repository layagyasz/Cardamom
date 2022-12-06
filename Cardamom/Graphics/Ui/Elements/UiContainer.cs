using Cardamom.Graphics.Ui.Controller;
using Cardamom.Planar;
using OpenTK.Graphics.OpenGL;

namespace Cardamom.Graphics.Ui.Elements
{
    public class UiContainer : SimpleUiElement, IEnumerable<IUiElement>
    {
        private readonly List<IUiElement> _elements = new();

        public UiContainer(Class @class, IController controller)
            : base(@class, controller) { }

        public override void Initialize()
        {
            base.Initialize();
            _elements.ForEach(x => x.Initialize());
        }

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

        public override void Draw(RenderTarget target)
        {
            if (Visible)
            {
                base.Draw(target);
                target.PushTranslation(Position + LeftMargin + LeftPadding);
                target.PushScissor(new(new(), InternalSize));
                foreach (var element in _elements)
                {
                    element.Draw(target);
                }
                target.PopScissor();
                target.PopTransform();
            }
        }

        public override void Update(UiContext context, long delta)
        {
            base.Update(context, delta);
            context.PushTranslation(Position + LeftMargin + LeftPadding);
            context.PushScissor(new(new(), InternalSize));
            _elements.ForEach(x => x.Update(context, delta));
            context.PopScissor();
            context.PopTransform();
        }
    }
}
