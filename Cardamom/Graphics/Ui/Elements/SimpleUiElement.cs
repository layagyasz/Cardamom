using Cardamom.Graphics.Ui.Controller;
using Cardamom.Graphics.Ui.Elements.Components;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements
{
    public class SimpleUiElement : ClassedUiElement
    {
        private readonly RectangleComponent _rectComponent = new();

        public SimpleUiElement(Class @class, IController controller)
            : base(@class, controller) 
        {
            SetAttributes(@class.Get(Class.State.NONE));
        }

        public override void Draw(RenderTarget target)
        {
            if (Visible)
            {
                target.PushTranslation(Position + LeftMargin);
                _rectComponent.Draw(target);
                target.PopTransform();
            }
        }

        public override void Update(UiContext context, long delta)
        {
            context.PushTranslation(Position + LeftMargin);
            base.Update(context, delta);
            context.PopTransform();
        }

        public override bool IntersectsRay(Vector3 origin, Vector3 direction)
        {
            return _rectComponent.IntersectsRay(origin, direction);
        }

        public override void SetAttributes(ClassAttributes attributes)
        {
            base.SetAttributes(attributes);
            _rectComponent.SetAttributes(attributes);
        }
    }
}
