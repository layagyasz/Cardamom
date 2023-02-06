﻿using Cardamom.Graphics;
using Cardamom.Mathematics.Geometry;
using Cardamom.Ui.Controller.Element;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class InteractiveModel<T> : IInteractive, IRenderable where T : struct
    {
        public IElementController Controller { get; }
        public IControlledElement? Parent { get; set; }

        private readonly Model<T> _model;
        private readonly ICollider3 _collider;

        public InteractiveModel(Model<T> model, ICollider3 collider, IElementController controller)
        {
            Controller = controller;
            _model = model;
            _collider = collider;
        }

        public void Initialize()
        {
            Controller.Bind(this);
            _model.Initialize();
        }

        public void ResizeContext(Vector3 bounds)
        {
            _model.ResizeContext(bounds);
        }

        public float? GetRayIntersection(Ray3 ray)
        {
            return _collider.GetRayIntersection(ray);
        }

        public void Draw(RenderTarget target, UiContext context)
        {
            context.Register(this);
            _model.Draw(target, context);
        }

        public virtual void Update(long delta)
        {
            _model.Update(delta);
        }
    }
}