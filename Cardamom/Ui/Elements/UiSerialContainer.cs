﻿using Cardamom.Graphics;
using Cardamom.Ui.Controller.Element;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class UiSerialContainer : BaseOffsetUiContainer, IUiContainer
    {
        public enum Orientation
        {
            Horizontal,
            Vertical
        }

        private readonly Orientation _orientation;

        public UiSerialContainer(Class @class, IElementController controller, Orientation orientation)
            : base(@class, controller)
        {
            _orientation = orientation;
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
            float offset = 0;
            Box3 bounds = new();
            foreach (var element in this)
            {
                if (element.Visible)
                {
                    element.Position = MapOffset(offset);
                    element.OverrideDepth = OverrideDepth;

                    offset += _orientation == Orientation.Vertical ? element.Size.Y : element.Size.X;
                    bounds.Inflate(element.Position + element.Size);
                    element.Draw(target, context);
                }
            }
            SetDynamicSize(bounds.Size);
            SetMaxOffset(
                Math.Min(0, (_orientation == Orientation.Vertical ? InternalSize.Y : InternalSize.X) - offset));
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
            return _orientation == Orientation.Horizontal ? new(offset, 0, 0) : new(0, offset, 0);
        }
    }
}
