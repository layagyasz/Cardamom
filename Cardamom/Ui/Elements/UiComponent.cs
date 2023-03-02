using Cardamom.Graphics;
using Cardamom.Ui.Controller;
using Cardamom.Ui.Controller.Element;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class UiComponent : GraphicsResource, IUiContainer
    {
        public EventHandler<ElementEventArgs>? ElementAdded { get; set; }

        public IController ComponentController { get; }
        public IElementController Controller => _container.Controller;
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

        private IUiContainer _container;

        public UiComponent(IController componentController, IUiContainer container)
        {
            ComponentController = componentController;
            _container = container;
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
        }

        public void Draw(RenderTarget target, UiContext context)
        {
            _container.Draw(target, context);
        }

        public IEnumerator<IRenderable> GetEnumerator()
        {
            return _container.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
    }
}
