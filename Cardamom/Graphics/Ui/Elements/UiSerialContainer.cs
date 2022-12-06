using Cardamom.Graphics.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements
{
    public class UiSerialContainer : SimpleUiElement, IEnumerable<IUiElement>
    {
        private readonly List<IUiElement> _elements = new();
        private Vector2 _offset;
        private float _maxOffset;

        public UiSerialContainer(Class @class, IController controller)
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

        public void TryAdjustOffset(float Amount)
        {
            _offset = new(0, Math.Min(Math.Max(_offset.Y + Amount, _maxOffset), 0));
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
            base.Draw(target);
            target.PushTranslation(Position + LeftMargin + LeftPadding);
            target.PushScissor(new(new(), InternalSize));
            target.PushTranslation(_offset);
            foreach (var element in _elements)
            {
                element.Draw(target);
            }
            target.PopTransform();
            target.PopScissor();
            target.PopTransform();
        }

        public override void Update(UiContext context, long delta)
        {
            base.Update(context, delta);
            context.PushTranslation(Position + LeftMargin + LeftPadding);
            context.PushScissor(new(new(), InternalSize));
            context.PushTranslation(_offset);
            float offset = 0;
            foreach (var element in _elements)
            {
                element.Position = new(0, offset);
                _maxOffset = -offset;
                offset += element.Size.Y;
                element.Update(context, delta);
            }
            context.PopTransform();
            context.PopScissor();
            context.PopTransform();
        }
    }
}
