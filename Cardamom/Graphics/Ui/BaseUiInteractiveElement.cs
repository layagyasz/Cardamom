using Cardamom.Graphics.Ui.Controller;
using Cardamom.Mathematics.Geometry;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
{
    public abstract class BaseUiInteractiveElement : IInteractive, IUiElement
    {
        public bool Visible { get; set; } = true;
        public IController Controller { get; }
        public Vector3 Position { get; set; }
        public abstract Vector3 Size { get; }
        public IControlled? Parent { get; set; }

        public BaseUiInteractiveElement(IController controller)
        {
            Controller = controller;
        }

        public virtual void Initialize()
        {
            Controller.Bind(this);
        }

        public abstract float? GetRayIntersection(Ray3 ray);

        public abstract void Draw(RenderTarget target);

        public virtual void Update(UiContext context, long delta)
        {
            if (Visible)
            {
                context.Register(this);
            }
        }
    }
}
