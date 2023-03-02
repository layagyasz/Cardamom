using Cardamom.Graphics;
using Cardamom.Ui.Controller.Element;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class UiContainer : SimpleUiElement, IUiContainer
    {
        private class UiElementComparer : IComparer<Vector3>
        {
            public int Compare(Vector3 left, Vector3 right)
            {
                int cz = left.Z.CompareTo(right.Z);
                if (cz != 0)
                {
                    return cz;
                }
                int cy = left.Y.CompareTo(right.Y);
                if (cy != 0)
                {
                    return -cy;
                }
                return left.X.CompareTo(right.X);
            }
        }

        private readonly SortedList<Vector3, IUiElement> _elements = new(new UiElementComparer());

        public UiContainer(Class @class, IElementController controller)
            : base(@class, controller) { }

        public override void Initialize()
        {
            base.Initialize();
            foreach (var element in _elements.Values)
            {
                element.Initialize();
            }
        }

        public void Add(IUiElement element)
        {
            _elements.Add(element.Position, element);
            element.Parent = this;
        }

        public IEnumerator<IRenderable> GetEnumerator()
        {
            return _elements.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Draw(RenderTarget target, UiContext context)
        {
            if (Visible)
            {
                base.Draw(target, context);
                target.PushTranslation(Position + LeftMargin);
                target.PushTranslation(LeftPadding);
                if (!DisableScissor)
                {
                    target.PushScissor(new(new(), InternalSize));
                }
                Box3 bounds = new();
                foreach (var element in _elements.Values)
                {
                    bounds.Inflate(element.Position + element.Size);
                    element.Draw(target, context);
                }
                SetDynamicSize(bounds.Size + LeftPadding + RightPadding);
                if (!DisableScissor)
                {
                    target.PopScissor();
                }
                target.PopModelMatrix();
                target.PopModelMatrix();
            }
        }

        public override void Update(long delta)
        {
            foreach (var element in _elements.Values)
            {
                element.Update(delta);
            }
        }
    }
}
