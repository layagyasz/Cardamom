using Cardamom.Graphics;
using Cardamom.Mathematics.Geometry;
using Cardamom.Ui.Controller.Element;
using Cardamom.Ui.Elements.Components;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class SimpleUiElement : ClassedUiElement
    {
        private readonly RectangleComponent _rectComponent = new();

        public SimpleUiElement(Class @class, IElementController controller)
            : base(@class, controller)
        {
            SetAttributes(@class.Get(Class.State.None));
        }

        public override void Draw(IRenderTarget target, IUiContext context)
        {
            if (Visible)
            {
                target.PushTranslation(Position + LeftMargin);
                context.Register(this);
                _rectComponent.Draw(target);
                target.PopModelMatrix();
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
            SetDyamicSizeImpl(TrueSize.Xy);
        }

        protected override void SetDyamicSizeImpl(Vector2 size)
        {
            _rectComponent.SetSize(size);
        }

        protected override void DisposeImpl()
        {
            _rectComponent.Dispose();
        }
    }
}
