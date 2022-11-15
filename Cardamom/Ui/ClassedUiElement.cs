using Cardamom.Ui.Controller;
using SFML.Graphics;
using SFML.System;

namespace Cardamom.Ui
{
    public abstract class ClassedUiElement : BaseUiInteractiveElement
    {
        public Vector2f LeftMargin { get; private set; }
        public Vector2f RightMargin { get; private set; }
        public Vector2f LeftPadding { get; private set; }
        public Vector2f RightPadding { get; private set; }

        private readonly Class _class;
        private Class.State _state;

        public ClassedUiElement(Class @class, IController controller)
            : base(controller)
        {
            _class = @class;
            _state = Class.State.NONE;
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
            _state = state;
            SetAttributes(_class.Get(state));
        }

        public override void Update(UiContext context, Transform transform, long delta)
        {
            transform.Translate(LeftMargin);
            base.Update(context, transform, delta);
        }
    }
}
