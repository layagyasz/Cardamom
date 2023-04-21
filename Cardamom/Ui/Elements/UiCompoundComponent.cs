using Cardamom.Graphics;
using Cardamom.Ui.Controller;
using Cardamom.Ui.Controller.Element;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class UiCompoundComponent : GraphicsResource, IUiContainer
    {
        public EventHandler<ElementEventArgs>? ElementAdded { get; set; }
        public EventHandler<ElementEventArgs>? ElementRemoved { get; set; }

        public IController ComponentController { get; }
        public IElementController Controller => _container.Controller;
        public int Count => _container.Count;
        public IControlledElement? Parent
        {
            get => _container.Parent;
            set => _container.Parent = value;
        }
        public Vector3 Position
        {
            get => _container.Position;
            set => _container.Position = value;
        }

        public Vector3 Size => _container.Size;
        public bool Visible
        {
            get => _container.Visible;
            set => _container.Visible = value;
        }

        protected readonly IUiContainer _container;

        public UiCompoundComponent(IController componentController, IUiContainer container)
        {
            ComponentController = componentController;
            _container = container;
        }

        public void Add(IUiElement element)
        {
            _container.Add(element);
        }

        public void Clear(bool dispose)
        {
            _container.Clear(dispose);
        }

        protected override void DisposeImpl()
        {
            _container.Dispose();
        }

        public void Initialize()
        {
            _container.Initialize();
            ComponentController.Bind(this);
            _container.ElementAdded += HandleElementAdded;
            _container.ElementRemoved += HandleElementRemoved;
        }

        public virtual void Draw(IRenderTarget target, IUiContext context)
        {
            _container.Draw(target, context);
        }

        public IUiContainer GetContainer()
        {
            return _container;
        }

        public IEnumerator<IUiElement> GetEnumerator()
        {
            return _container.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Remove(IUiElement element)
        {
            _container.Remove(element);
        }

        public void ResizeContext(Vector3 bounds)
        {
            _container.ResizeContext(bounds);
        }

        public void Update(long delta)
        {
            _container.Update(delta);
        }

        private void HandleElementAdded(object? sender, ElementEventArgs e)
        {
            ElementAdded?.Invoke(this, e);
        }

        private void HandleElementRemoved(object? sender, ElementEventArgs e)
        {
            ElementRemoved?.Invoke(this, e);
        }
    }
}
