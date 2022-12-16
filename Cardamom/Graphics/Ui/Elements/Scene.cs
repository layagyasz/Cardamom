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
        public Vector3 Size { get; }
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
            Controller.Bind(this);
        }

        public void Draw(RenderTarget target)
        {
            if (Visible)
            {
                target.PushTranslation(Position);
                target.PushTransform(Camera.GetViewMatrix());
                foreach (var element in _elements)
                {
                    element.Draw(target);
                }
                target.PopTransform();
                target.PopTransform();
            }
        }

        public void Update(UiContext context, long delta)
        {
            if (Visible)
            {
                context.PushTranslation(Position);
                context.PushTransform(Camera.GetViewMatrix());
                foreach (var element in _elements)
                {
                    element.Update(context, delta);
                }
                context.PopTransform();
                context.PopTransform();
            }
        }
    }
}
