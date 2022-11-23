using Cardamom.Graphics;
using Cardamom.Planar;
using Cardamom.Ui.Controller;
using Cardamom.Ui.Elements.Components;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class SimpleUiElement : ClassedUiElement
    {
        private readonly RectangleComponent _rectComponent = new();

        public override Vector2 Size => _rectComponent.Size + LeftMargin + RightMargin;

        public SimpleUiElement(Class @class, IController controller)
            : base(@class, controller) 
        {
            SetAttributes(@class.Get(Class.State.NONE));
        }

        public override void Draw(RenderTarget target, Transform2 transform)
        {
            if (Visible)
            {
                transform.Translate(Position + LeftMargin);
                _rectComponent.Draw(target, transform);
            }
        }

        public override bool IsPointWithinBounds(Vector2 point)
        {
            return _rectComponent.IsPointWithinBounds(point);
        }

        public override void SetAttributes(ClassAttributes attributes)
        {
            _rectComponent.SetAttributes(attributes);
        }
    }
}
