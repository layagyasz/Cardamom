using Cardamom.Graphics.Ui.Controller;
using Cardamom.Graphics.Ui.Elements.Components;
using Cardamom.Planar;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements
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
