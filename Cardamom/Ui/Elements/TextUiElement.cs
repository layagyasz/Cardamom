using Cardamom.Graphics;
using Cardamom.Mathematics.Geometry;
using Cardamom.Ui.Controller.Element;
using Cardamom.Ui.Elements.Components;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class TextUiElement : ClassedUiElement
    {
        protected readonly RectangleComponent _rectComponent = new();
        protected readonly TextComponent _textComponent = new();

        protected Vector3 _alignAdjust;

        private ClassAttributes.Alignment _align;
        private string _text = string.Empty;

        public TextUiElement(Class @class, IElementController controller)
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
                target.PushTranslation(LeftPadding + _alignAdjust);
                if (!DisableScissor)
                {
                    target.PushScissor(new(new(), InternalSize));
                }
                _textComponent.Draw(target, context);
                if (!DisableScissor)
                {
                    target.PopScissor();
                }
                target.PopModelMatrix();
                target.PopModelMatrix();

                SetDynamicSize(LeftPadding + RightPadding + new Vector3(_textComponent.Size));
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
            _align = attributes.Align;
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
            _alignAdjust = GetAlign();
        }

        public string GetText()
        {
            return _text;
        }

        protected Vector3 GetAlign()
        {
            switch (_align)
            {
                case ClassAttributes.Alignment.Left:
                    return new();
                case ClassAttributes.Alignment.Center:
                    return new(0.5f * (InternalSize.X - _textComponent.Size.X), 0, 0);
                case ClassAttributes.Alignment.Right:
                    return new(InternalSize.X - _textComponent.Size.X, 0, 0);
                default:
                    throw new InvalidProgramException();
            }
        }

        protected override void DisposeImpl()
        {
            _rectComponent.Dispose();
            _textComponent.Dispose();
        }
    }
}
