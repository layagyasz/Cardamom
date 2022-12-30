using Cardamom.Graphics.Ui.Controller;
using Cardamom.Mathematics.Geometry;

namespace Cardamom.Graphics.Ui.Elements
{
    public class InteractiveModel : IInteractive, IRenderable
    {
        public IController Controller { get; }
        public IControlled? Parent { get; set; }

        private readonly Model _model;
        private readonly ICollider3 _collider;

        public InteractiveModel(Model model, ICollider3 collider, IController controller)
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
