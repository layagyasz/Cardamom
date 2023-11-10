using Cardamom.Graphics;
using Cardamom.Ui.Controller.Element;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class UiChipContainer : BaseOffsetUiContainer, IUiContainer
    {
        private float _maxWidth;

        public UiChipContainer(Class @class, IElementController controller)
            : base(@class, controller) { }

        public override void SetAttributes(ClassAttributes attributes)
        {
            base.SetAttributes(attributes);
            _maxWidth = attributes.Size.Width.GetMaxSize() - attributes.LeftPadding.X - attributes.LeftPadding.X;
        }

        public override void Draw(IRenderTarget target, IUiContext context)
        {
            if (!Visible)
            {
                return;
            }
            base.Draw(target, context);
            target.PushTranslation(Position + LeftMargin + LeftPadding);
            if (!DisableScissor)
            {
                target.PushScissor(new(new(), InternalSize));
            }
            target.PushTranslation(Offset);
            Vector3 cursor = new();
            Box3 bounds = new();
            foreach (var element in this)
            {
                if (element.Visible)
                {
                    if (cursor.X + element.Size.X > _maxWidth)
                    {
                        cursor = new(0, bounds.Size.Y, 0);
                    }
                    element.Position = cursor;
                    element.OverrideDepth = OverrideDepth;
                    cursor.X += element.Size.X;
                    bounds.Inflate(element.Position + element.Size);
                    element.Draw(target, context);
                }
            }
            SetDynamicSize(bounds.Size);
            SetMaxOffset(Math.Min(0, InternalSize.Y - bounds.Size.Y));
            TryAdjustOffset(0);
            target.PopModelMatrix();
            if (!DisableScissor)
            {
                target.PopScissor();
            }
            target.PopModelMatrix();
        }

        protected override Vector3 MapOffset(float offset)
        {
            return new(0, offset, 0);
        }
    }
}
