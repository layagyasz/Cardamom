using Cardamom.Ui.Controller;
using Cardamom.Ui.Elements.Components;
using SFML.Graphics;
using SFML.System;

namespace Cardamom.Ui.Elements
{
    public class SimpleUiElement : ClassedUiElement
    {
        private readonly RectComponent _rectComponent = new();

        public override Vector2f Size => _rectComponent.Size + LeftMargin + RightMargin;

        public SimpleUiElement(Class @class, IController controller)
            : base(@class, controller) 
        {
            SetAttributes(@class.Get(Class.State.NONE));
        }

        public override void Draw(RenderTarget target, Transform transform)
        {
            transform.Translate(Position + LeftMargin);
            _rectComponent.Draw(target, transform);
        }

        public override bool IsPointWithinBounds(Vector2f point)
        {
            return _rectComponent.IsPointWithinBounds(point);
        }

        public override void SetAttributes(ClassAttributes attributes)
        {
            _rectComponent.SetAttributes(attributes);
        }
    }
}
