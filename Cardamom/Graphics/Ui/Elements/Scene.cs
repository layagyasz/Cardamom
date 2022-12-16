using Cardamom.Graphics.Camera;
using Cardamom.Graphics.Ui.Controller;
using System.Net.Mime;

namespace Cardamom.Graphics.Ui.Elements
{
    public class Scene : IRenderable, IControlled
    {
        public IController Controller { get; }
        public IControlled? Parent { get; set; }
        public ICamera Camera { get; }

        private List<IRenderable> _elements;

        public Scene(IController controller, ICamera camera, IEnumerable<IRenderable> elements)
        {
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
            target.PushTransform(Camera.GetViewMatrix());
            foreach (var element in _elements)
            {
                element.Draw(target);
            }
            target.PopTransform();
        }

        public void Update(UiContext context, long delta)
        {
            context.PushTransform(Camera.GetViewMatrix());
            foreach (var element in _elements)
            {
                element.Update(context, delta);
            }
            context.PopTransform();
        }
    }
}
