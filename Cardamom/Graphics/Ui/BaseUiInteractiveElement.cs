using Cardamom.Graphics.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
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
