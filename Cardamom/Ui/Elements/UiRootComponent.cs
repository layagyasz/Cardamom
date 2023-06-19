using Cardamom.Graphics;
using Cardamom.Ui.Controller.Element;
using Cardamom.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class UiRootComponent : GraphicsResource, IUiContainer, IUiComponent
    {
        public EventHandler<ElementEventArgs>? ElementAdded { get; set; }
        public EventHandler<ElementEventArgs>? ElementRemoved { get; set; }

        public IController ComponentController { get; }
        public IElementController Controller => _root.Controller;
        public int Count => _children.Count + 1;
        public IControlledElement? Parent
        {
            get => _root.Parent;
            set => _root.Parent = value;
        }
        public Vector3 Position
        {
            get => _root.Position;
            set => _root.Position = value;
        }

        public Vector3 Size => _root.Size;
        public bool Visible
        {
            get => _root.Visible;
            set => _root.Visible = value;
        }
        public float? OverrideDepth
        {
            get => _root.OverrideDepth;
            set => _root.OverrideDepth = value;
        }

        protected readonly IUiElement _root;
        protected readonly List<IUiElement> _children = new();

        public UiRootComponent(IController componentController, IUiElement root)
        {
            ComponentController = componentController;
            _root = root;
        }

        public void Add(IUiElement element)
        {
            element.Parent = _root;
            _children.Add(element);
            ElementAdded?.Invoke(this, new(element));
        }

        public void Clear(bool dispose)
        {
            if (dispose)
            {
                foreach (var child in _children)
                {
                    child.Dispose();
                }
            }
            _children.Clear();
        }

        protected override void DisposeImpl()
        {
            _root.Dispose();
            foreach (var child in _children)
            {
                child.Dispose();
            }
        }

        public virtual void Draw(IRenderTarget target, IUiContext context)
        {
            _root.Draw(target, context);
            foreach (var child in _children)
            {
                child.Draw(target, context);
            }
        }

        public IEnumerator<IUiElement> GetEnumerator()
        {
            yield return _root;
            foreach (var child in _children)
            {
                yield return child;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Initialize()
        {
            _root.Initialize();
            foreach (var child in _children)
            {
                child.Initialize();
            }
            ComponentController.Bind(this);
        }

        public void Insert(int index, IUiElement element)
        {
            _children.Insert(index, element);
            ElementAdded?.Invoke(this, new(element));
        }

        public void Remove(IUiElement element)
        {
            _children.Remove(element);
            ElementRemoved?.Invoke(this, new(element));
        }

        public virtual void ResizeContext(Vector3 bounds) { }

        public void Update(long delta)
        {
            _root.Update(delta);
            foreach (var child in _children)
            {
                child.Update(delta);
            }
        }
    }
}
