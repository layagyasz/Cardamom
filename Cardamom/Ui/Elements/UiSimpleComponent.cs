using Cardamom.Graphics;
using Cardamom.Ui.Controller.Element;
using Cardamom.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class UiSimpleComponent : ManagedResource, IUiComponent
    {
        public IController ComponentController { get; }
        public IElementController Controller => _element!.Controller;
        public IControlledElement? Parent
        {
            get => _element!.Parent;
            set => _element!.Parent = value;
        }
        public Vector3 Position
        {
            get => _element!.Position;
            set => _element!.Position = value;
        }

        public Vector3 Size => _element!.Size;
        public bool Visible
        {
            get => _element!.Visible;
            set => _element!.Visible = value;
        }
        public float? OverrideDepth
        {
            get => _element!.OverrideDepth;
            set => _element!.OverrideDepth = value;
        }

        protected IUiElement? _element;

        public UiSimpleComponent(IController componentController, IUiElement element)
        {
            ComponentController = componentController;
            _element = element;
        }

        protected override void DisposeImpl()
        {
            _element!.Dispose();
        }

        public virtual void Draw(IRenderTarget target, IUiContext context)
        {
            _element!.Draw(target, context);
        }

        public IUiElement GetElement()
        {
            return _element!;
        }

        public void Initialize()
        {
            _element?.Initialize();
            ComponentController.Bind(this);
        }

        public virtual void ResizeContext(Vector3 bounds)
        {
            _element!.ResizeContext(bounds);
        }

        public void Update(long delta)
        {
            _element!.Update(delta);
        }
    }
}
