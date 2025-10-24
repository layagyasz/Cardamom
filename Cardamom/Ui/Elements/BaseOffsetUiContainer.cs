using Cardamom.Ui.Controller.Element;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public abstract class BaseOffsetUiContainer : SimpleUiElement, IUiContainer
    {
        public EventHandler<ElementEventArgs>? ElementAdded { get; set; }
        public EventHandler<ElementEventArgs>? ElementRemoved { get; set; }

        public int Count => _elements.Count;
        public Vector3 Offset { get; private set; }

        private readonly List<IUiElement> _elements = new();

        private float _offsetValue;
        private float _maxOffset;

        protected BaseOffsetUiContainer(Class @class, IElementController controller)
            : base(@class, controller) { }

        public void Add(IUiElement element)
        {
            _elements.Add(element);
            element.Parent = this;
            ElementAdded?.Invoke(this, new(element));
        }

        public void Clear(bool dispose)
        {
            foreach (var element in _elements.ToList())
            {
                ElementRemoved?.Invoke(this, new(element));
                if (dispose)
                {
                    element.Dispose();
                }
            }
            _elements.Clear();
            Offset = new();
        }

        public IEnumerator<IUiElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Initialize()
        {
            base.Initialize();
            _elements.ForEach(x => x.Initialize());
        }

        public void Insert(int index, IUiElement element)
        {
            _elements.Insert(index, element);
            element.Parent = this;
            ElementAdded?.Invoke(this, new(element));
        }

        public void Remove(IUiElement element, bool dispose)
        {
            if (_elements.Remove(element))
            {
                ElementRemoved?.Invoke(this, new(element));
                if (dispose)
                {
                    element.Dispose();
                }
            }
        }

        public void SetOffset(float amount)
        {
            _offsetValue = MathHelper.Clamp(amount, _maxOffset, 0);
            Offset = MapOffset(_offsetValue);
        }

        public void Sort(IComparer<IUiElement> comparer)
        {
            _elements.Sort(comparer);
        }

        public bool TryAdjustOffset(float amount)
        {
            float newOffsetValue = Math.Clamp(_offsetValue + amount, _maxOffset, 0);
            bool changed = Math.Abs(_offsetValue - newOffsetValue) > float.Epsilon;
            _offsetValue = newOffsetValue;
            Offset = MapOffset(newOffsetValue);
            return changed;
        }

        public override void Update(long delta)
        {
            base.Update(delta);
            foreach (var element in _elements)
            {
                element.Update(delta);
            }
        }

        protected override void DisposeImpl()
        {
            base.DisposeImpl();
            foreach (var element in _elements)
            {
                element.Dispose();
            }
        }

        protected abstract Vector3 MapOffset(float offset);

        protected void SetMaxOffset(float maxOffset)
        {
            _maxOffset = maxOffset;
        }
    }
}
