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

        public EventHandler<ElementEventArgs>? ElementAdded { get; set; }
        public EventHandler<ElementEventArgs>? ElementRemoved { get; set; }

        public int Count => _elements.Count;

        protected readonly SortedList<Vector3, IUiElement> _elements = new(new UiElementComparer());

        public UiContainer(Class @class, IElementController controller)
            : base(@class, controller) { }

        public void Add(IUiElement element)
        {
            _elements.Add(element.Position, element);
            element.Parent = this;
            ElementAdded?.Invoke(this, new(element));
        }

        public void Clear(bool dispose)
        {
            foreach (var element in _elements.Values)
            {
                ElementRemoved?.Invoke(this, new(element));
                if (dispose)
                {
                    element.Dispose();
                }
            }
            _elements.Clear();
        }

        public override void Draw(IRenderTarget target, IUiContext context)
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
                    element.OverrideDepth = OverrideDepth;
                    element.Draw(target, context);
                }
                SetDynamicSize(bounds.Size);
                if (!DisableScissor)
                {
                    target.PopScissor();
                }
                target.PopModelMatrix();
                target.PopModelMatrix();
            }
        }

        public IEnumerator<IUiElement> GetEnumerator()
        {
            return _elements.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Initialize()
        {
            base.Initialize();
            foreach (var element in _elements.Values.ToList())
            {
                element.Initialize();
            }
        }

        public void Insert(int index, IUiElement element)
        {
            throw new NotSupportedException();
        }

        public void Remove(IUiElement element)
        {
            int index = _elements.IndexOfValue(element);
            if (index > -1)
            {
                _elements.RemoveAt(index);
                ElementRemoved?.Invoke(this, new(element));
            }
        }

        public override void Update(long delta)
        {
            foreach (var element in _elements.Values)
            {
                element.Update(delta);
            }
        }

        protected override void DisposeImpl()
        {
            base.DisposeImpl();
            foreach (var element in _elements)
            {
                element.Value.Dispose();
            }
        }
    }
}
