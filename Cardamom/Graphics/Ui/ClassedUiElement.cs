using Cardamom.Graphics.Ui.Controller;
using Cardamom.Planar;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
{
    public abstract class ClassedUiElement : BaseUiInteractiveElement
    {
        public Vector2 LeftMargin { get; private set; }
        public Vector2 RightMargin { get; private set; }
        public Vector2 LeftPadding { get; private set; }
        public Vector2 RightPadding { get; private set; }

        private readonly Class _class;

        public ClassedUiElement(Class @class, IController controller)
            : base(controller)
        {
            _class = @class;
        }

        public virtual void SetAttributes(ClassAttributes attributes)
        {
            LeftMargin = attributes.LeftMargin;
            RightMargin = attributes.RightMargin;
            LeftPadding = attributes.LeftPadding;
            RightPadding = attributes.RightPadding;
        }

        public void SetState(Class.State state)
        {
            SetAttributes(_class.Get(state));
        }

        public override void Update(UiContext context, Transform2 transform, long delta)
        {
            transform.Translate(LeftMargin);
            base.Update(context, transform, delta);
        }
    }
}
