using Cardamom.Graphics.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements
{
    public class UiSerialContainer : SimpleUiElement, IEnumerable<IUiElement>
    {
        private readonly List<IUiElement> _elements = new();
        private Vector3 _offset;
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
            _offset = new(0, Math.Min(Math.Max(_offset.Y + Amount, _maxOffset), 0), 0);
        }

        public IEnumerator<IUiElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Draw(RenderTarget target, UiContext context)
        {
            base.Draw(target, context);
            target.PushTranslation(Position + LeftMargin + LeftPadding);
            target.PushScissor(new(new(), InternalSize));
            target.PushTranslation(_offset);
            float offset = 0;
            foreach (var element in _elements)
            {
                element.Position = new(0, offset, Position.Z);
                _maxOffset = -offset;
                offset += element.Size.Y;
                element.Draw(target, context);
            }
            target.PopViewMatrix();
            target.PopScissor();
            target.PopViewMatrix();
        }

        public override void Update(long delta)
        {
            base.Update(delta);
            foreach (var element in _elements)
            {
                element.Update(delta);
            }
        }
    }
}
