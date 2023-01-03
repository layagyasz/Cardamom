using Cardamom.Graphics.Camera;
using Cardamom.Graphics.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements
{
    public class Scene : IUiElement
    {
        public IController Controller { get; }
        public IControlled? Parent { get; set; }
        public ICamera Camera { get; }
        public Vector3 Position { get; set; }
        public Vector3 Size { get; private set; }
        public bool Visible { get; set; } = true;

        private List<IRenderable> _elements;

        public Scene(Vector3 size, IController controller, ICamera camera, IEnumerable<IRenderable> elements)
        {
            Size = size;
            Controller = controller;
            Camera = camera;
            _elements = elements.ToList();
        }

        public void Initialize()
        {
            _elements.ForEach(x => x.Initialize());
            Controller.Bind(this);
        }

        public void ResizeContext(Vector3 bounds)
        {
            Size = bounds;
            Camera.SetAspectRatio(bounds.X / bounds.Y);
        }

        public void Draw(RenderTarget target, UiContext context)
        {
            if (Visible)
            {
                target.PushTranslation(Position);
                target.PushViewMatrix(Camera.GetViewMatrix());
                target.PushProjection(Camera.GetProjection());
                foreach (var element in _elements)
                {
                    element.Draw(target, context);
                }
                target.PopProjectionMatrix();
                target.PopViewMatrix();
                target.PopViewMatrix();
            }
        }

        public void Update(long delta)
        {
            if (Visible)
            {
                foreach (var element in _elements)
                {
                    element.Update(delta);
                }
            }
        }
    }
}
