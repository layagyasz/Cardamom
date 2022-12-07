﻿using Cardamom.Graphics.Ui.Controller;
using Cardamom.Graphics.Ui.Elements.Components;
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
            SetAttributes(@class.Get(Class.State.NONE));
        }

        public override void Draw(RenderTarget target)
        {
            if (Visible)
            {
                target.PushTranslation(Position + LeftMargin);
                _rectComponent.Draw(target);
                target.PushTranslation(LeftPadding);
                target.PushScissor(new(new(), InternalSize));
                _textComponent.Draw(target);
                target.PopScissor();
                target.PopTransform();
                target.PopTransform();
            }
        }

        public override void Update(UiContext context, long delta)
        {
            context.PushTranslation(Position + LeftMargin);
            base.Update(context, delta);
            context.PopTransform();
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
            _text = text;
            _textComponent.SetText(text);
        }

        public string GetText()
        {
            return _text;
        }
    }
}