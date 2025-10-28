using Cardamom.Graphics;
using Cardamom.Ui.Controller;
using Cardamom.Ui.Controller.Element;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class UiGroup : ManagedResource, IEnumerable<IControlledElement>, IRenderable, IControlledElement
    {
        public EventHandler<ElementEventArgs>? ElementAdded { get; set; }
        public EventHandler<ElementEventArgs>? ElementRemoved { get; set; }

        public IElementController Controller { get; } = new NoOpElementController();
        public IControlledElement? Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                foreach (var  element in _elements)
                {
                    element.Parent = _parent;
                }
            }
        }
        public IController GroupController { get; }

        private IControlledElement? _parent;
        protected readonly List<IControlledElement> _elements = new();

        public UiGroup(IController controller)
        {
            GroupController = controller;
        }

        protected override void DisposeImpl()
        {
            foreach (var element in _elements)
            {
                if (element is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            _elements.Clear();
        }

        public virtual void Initialize()
        {
            GroupController.Bind(this);
            _elements.ForEach(x => x.Initialize());
        }

        public void ResizeContext(Vector3 bounds) { }

        public void Add(IControlledElement element)
        {
            element.Parent = _parent;
            _elements.Add(element);
            ElementAdded?.Invoke(this, new(element));
        }

        public void Clear()
        {
            foreach (var element in _elements)
            {
                ElementRemoved?.Invoke(this, new(element));
            }
            _elements.Clear();
        }

        public void Remove(IControlledElement element)
        {
            if (_elements.Remove(element))
            {
                ElementRemoved?.Invoke(this, new(element));
            }
        }

        public IEnumerator<IControlledElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Draw(IRenderTarget target, IUiContext context)
        {
            foreach (var element in _elements)
            {
                element.Draw(target, context);
            }
        }

        public virtual void Update(long delta)
        {
            _elements.ForEach(x => x.Update(delta));
        }
    }
}
