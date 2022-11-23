using Cardamom.Graphics;
using Cardamom.Planar;
using Cardamom.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Ui
{
    public abstract class BaseUiInteractiveElement : IUiInteractiveElement
    {
        public bool Visible { get; set; } = true;
        public IController Controller { get; }
        public Vector2 Position { get; set; }
        public abstract Vector2 Size { get; }
        public IControlled? Parent { get; set; }

        public BaseUiInteractiveElement(IController controller)
        {
            Controller = controller;
        }

        public virtual void Initialize()
        {
            Controller.Bind(this);
        }

        public abstract bool IsPointWithinBounds(Vector2 point);

        public abstract void Draw(RenderTarget target, Transform2 transform);

        public virtual void Update(UiContext context, Transform2 transform, long delta)
        {
            if (Visible)
            {
                transform.Translate(Position);
                context.Register(this, transform);
            }
        }
    }
}
