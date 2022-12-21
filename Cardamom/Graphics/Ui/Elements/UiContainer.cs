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

        public override void Draw(RenderTarget target)
        {
            if (Visible)
            {
                base.Draw(target);
                target.PushTranslation(Position + LeftMargin + LeftPadding);
                target.PushScissor(new(new(), InternalSize));
                foreach (var element in _elements.Values)
                {
                    element.Draw(target);
                }
                target.PopScissor();
                target.PopViewMatrix();
            }
        }

        public override void Update(UiContext context, long delta)
        {
            base.Update(context, delta);
            context.PushTranslation(Position + LeftMargin + LeftPadding);
            context.PushScissor(new(new(), InternalSize));
            foreach (var element in _elements.Values)
            {
                element.Update(context, delta);
            }
            context.PopScissor();
            context.PopViewMatrix();
        }
    }
}
