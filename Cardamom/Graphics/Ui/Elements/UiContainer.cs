using Cardamom.Graphics.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements
{
    public class UiContainer : SimpleUiElement, IEnumerable<IUiElement>
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

        public UiContainer(Class @class, IController controller)
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

        public IEnumerator<IUiElement> GetEnumerator()
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
                foreach (var element in _elements.Values)
                {
                    element.Draw(target, context);
                }
                if (!DisableScissor)
                {
                    target.PopScissor();
                }
                target.PopViewMatrix();
                target.PopViewMatrix();
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
