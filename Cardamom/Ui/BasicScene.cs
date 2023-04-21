using Cardamom.Graphics;
using Cardamom.Graphics.Camera;
using Cardamom.Mathematics.Geometry;
using Cardamom.Ui.Controller.Element;
using OpenTK.Mathematics;

namespace Cardamom.Ui
{
    public class BasicScene : IScene
    {
        public IElementController Controller { get; }
        public IControlledElement? Parent { get; set; }
        public ICamera Camera { get; }

        private readonly List<IRenderable> _elements;

        public BasicScene(Vector3 size, IElementController controller, ICamera camera, IEnumerable<IRenderable> elements)
        {
            Controller = controller;
            Camera = camera;
            _elements = elements.ToList();
            ResizeContext(size);
        }

        public void Initialize()
        {
            _elements.ForEach(x => x.Initialize());
            Controller.Bind(this);
        }

        public float? GetRayIntersection(Ray3 ray)
        {
            return float.MaxValue;
        }

        public void ResizeContext(Vector3 bounds)
        {
            Camera.SetAspectRatio(bounds.X / bounds.Y);
        }

        public void Draw(IRenderTarget target, IUiContext context)
        {
            target.PushViewMatrix(Camera.GetViewMatrix());
            target.PushProjection(Camera.GetProjection());
            context.Register(this);
            foreach (var element in _elements)
            {
                element.Draw(target, context);
            }
            target.PopProjectionMatrix();
            target.PopViewMatrix();
        }

        public void Update(long delta)
        {
            foreach (var element in _elements)
            {
                element.Update(delta);
            }
        }
    }
}
