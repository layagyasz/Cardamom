﻿using Cardamom.Graphics.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
{
    public abstract class ClassedUiElement : BaseUiInteractiveElement
    {
        public Vector2 LeftMargin { get; private set; }
        public Vector2 RightMargin { get; private set; }
        public Vector2 LeftPadding { get; private set; }
        public Vector2 RightPadding { get; private set; }
        public Vector2 TrueSize { get; private set; }
        public Vector2 InternalSize => TrueSize - LeftPadding - RightPadding;
        public override Vector2 Size => TrueSize + LeftMargin + RightMargin;

        private readonly Class _class;

        public ClassedUiElement(Class @class, IController controller)
            : base(controller)
        {
            _class = @class;
        }

        public virtual void SetAttributes(ClassAttributes attributes)
        {
            LeftMargin = attributes.LeftMargin;
            RightMargin = attributes.RightMargin;
            LeftPadding = attributes.LeftPadding;
            RightPadding = attributes.RightPadding;
            TrueSize = attributes.Size;
        }

        public void SetState(Class.State state)
        {
            SetAttributes(_class.Get(state));
        }
    }
}
