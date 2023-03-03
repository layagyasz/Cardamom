using Cardamom.Graphics;
using Cardamom.Ui.Controller.Element;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class UiSerialContainer : SimpleUiElement, IUiContainer
    {
        public enum Orientation
        {
            Horizontal,
            Vertical
        }

        public EventHandler<ElementEventArgs>? ElementAdded { get; set; }
        public EventHandler<ElementEventArgs>? ElementRemoved { get; set; }

        protected readonly List<IUiElement> _elements = new();
        private readonly Orientation _orientation;

        private Vector3 _offset;
        private float _maxOffset;

        public UiSerialContainer(Class @class, IElementController controller, Orientation orientation)
            : base(@class, controller)
        {
            _orientation = orientation;
        }

        public override void Initialize()
        {
            base.Initialize();
            _elements.ForEach(x => x.Initialize());
        }

        public void Add(IUiElement element)
        {
            _elements.Add(element);
            element.Parent = this;
            ElementAdded?.Invoke(this, new(element));
        }

        public void TryAdjustOffset(float Amount)
        {
            if (_orientation == Orientation.Vertical)
            {
                _offset = new(0, Math.Min(Math.Max(_offset.Y + Amount, _maxOffset), 0), 0);
            }
            else
            {
                _offset = new(Math.Min(Math.Max(_offset.X + Amount, _maxOffset), 0), 0, 0);
            }
        }

        public IEnumerator<IRenderable> GetEnumerator()
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
            if (!DisableScissor)
            {
                target.PushScissor(new(new(), InternalSize));
            }
            target.PushTranslation(_offset);
            float offset = 0;
            Box3 bounds = new();
            foreach (var element in _elements)
            {
                element.Position =
                    _orientation == Orientation.Vertical
                        ? new(0, offset, Position.Z) : new(offset, 0, Position.Z);
                _maxOffset = -offset;
                offset += _orientation == Orientation.Vertical ? element.Size.Y : element.Size.X;
                bounds.Inflate(element.Position + element.Size);
                element.Draw(target, context);
            }
            SetDynamicSize(bounds.Size + LeftPadding + RightPadding);
            target.PopModelMatrix();
            if (!DisableScissor)
            {
                target.PopScissor();
            }
            target.PopModelMatrix();
        }

        public void Remove(IUiElement element)
        {
            if (_elements.Remove(element))
            {
                ElementRemoved?.Invoke(this, new(element));
            }
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
