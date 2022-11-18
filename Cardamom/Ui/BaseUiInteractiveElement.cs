﻿using Cardamom.Ui.Controller;
using SFML.Graphics;
using SFML.System;

namespace Cardamom.Ui
{
    public abstract class BaseUiInteractiveElement : IUiInteractiveElement
    {
        public bool Visible { get; set; } = true;
        public IController Controller { get; }
        public Vector2f Position { get; set; }
        public abstract Vector2f Size { get; }
        public IControlled? Parent { get; set; }

        public BaseUiInteractiveElement(IController controller)
        {
            Controller = controller;
            Controller.Bind(this);
        }

        public abstract bool IsPointWithinBounds(Vector2f point);

        public abstract void Draw(RenderTarget target, Transform transform);

        public virtual void Update(UiContext context, Transform transform, long delta)
        {
            if (Visible)
            {
                transform.Translate(Position);
                context.Register(this, transform);
            }
        }
    }
}
