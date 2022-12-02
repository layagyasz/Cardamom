using Cardamom.Graphics.Ui.Controller;
using Cardamom.Graphics.Ui.Elements.Components;
using Cardamom.Planar;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements
{
    public class TextUiElement : ClassedUiElement
    {
        private readonly RectangleComponent _rectComponent = new();
        private readonly TextComponent _textComponent = new();

        public override Vector2 Size => _rectComponent.Size + LeftMargin + RightMargin;

        public TextUiElement(Class @class, IController controller)
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
                transform.Translate(LeftPadding);
                _textComponent.Draw(target, transform);
            }
        }

        public override bool IsPointWithinBounds(Vector2 point)
        {
            return _rectComponent.IsPointWithinBounds(point);
        }

        public override void SetAttributes(ClassAttributes attributes)
        {
            base.SetAttributes(attributes);
            _rectComponent.SetAttributes(attributes);
            _textComponent.SetAttributes(attributes);
        }

        public void SetText(string text)
        {
            _textComponent.SetText(text);
        }
    }
}
