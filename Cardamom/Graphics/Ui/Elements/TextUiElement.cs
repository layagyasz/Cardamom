using Cardamom.Graphics.Ui.Controller;
using Cardamom.Graphics.Ui.Elements.Components;
using Cardamom.Mathematics.Geometry;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements
{
    public class TextUiElement : ClassedUiElement
    {
        protected readonly RectangleComponent _rectComponent = new();
        protected readonly TextComponent _textComponent = new();

        private string _text = string.Empty;

        public TextUiElement(Class @class, IController controller)
            : base(@class, controller)
        {
            SetAttributes(@class.Get(Class.State.None));
        }

        public override void Draw(RenderTarget target, UiContext context)
        {
            if (Visible)
            {
                target.PushTranslation(Position + LeftMargin);
                context.Register(this);
                _rectComponent.Draw(target);
                target.PushTranslation(LeftPadding);
                if (!DisableScissor)
                {
                    target.PushScissor(new(new(), InternalSize));
                }
                _textComponent.Draw(target, context);
                if (!DisableScissor)
                {
                    target.PopScissor();
                }
                target.PopViewMatrix();
                target.PopViewMatrix();
            }
        }

        public override void Update(long delta) { }

        public override float? GetRayIntersection(Ray3 ray)
        {
            return _rectComponent.GetRayIntersection(ray);
        }

        public override void SetAttributes(ClassAttributes attributes)
        {
            base.SetAttributes(attributes);
            _rectComponent.SetAttributes(attributes);
            _textComponent.SetAttributes(attributes);
            SetDyamicSizeImpl(TrueSize.Xy);
        }

        protected override void SetDyamicSizeImpl(Vector2 size)
        {
            _rectComponent.SetSize(size);
        }

        public void SetText(string text)
        {
            _text = text;
            _textComponent.SetText(text);
        }

        public string GetText()
        {
            return _text;
        }

        protected override void DisposeImpl()
        {
            _rectComponent.Dispose();
        }
    }
}
