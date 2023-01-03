﻿using Cardamom.Graphics.Ui.Controller;
using Cardamom.Graphics.Ui.Elements.Components;
using Cardamom.Mathematics.Geometry;

namespace Cardamom.Graphics.Ui.Elements
{
    public class SimpleUiElement : ClassedUiElement
    {
        private readonly RectangleComponent _rectComponent = new();

        public SimpleUiElement(Class @class, IController controller)
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
        }

        protected override void DisposeImpl()
        {
            _rectComponent.Dispose();
        }
    }
}
