using Cardamom.Graphics.Ui.Controller;
using Cardamom.Planar;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements
{
    public class Screen : IUiInteractiveElement
    {
        public bool Visible { get; set; } = true;
        public Vector3 Size => new(_bounds.Size.X, _bounds.Size.Y, 0);
        public Vector3 Position { get; set; }
        public IController Controller { get; set; }
        public IControlled? Parent { get; set; }

        private readonly UiLayer[] _uiLayers;

        private readonly Rectangle _bounds;

        public Screen(Rectangle bounds, IController controller, IEnumerable<UiLayer> UiLayers)
        {
            Controller = controller;
            _bounds = bounds;
            _uiLayers = UiLayers.ToArray();
        }

        public void Initialize()
        {
            foreach (var layer in _uiLayers)
            {
                layer.Initialize();
            }
            Controller.Bind(this);
        }

        public bool IntersectsRay(Vector3 origin, Vector3 direction)
        {
            return true;
        }

        public void Draw(RenderTarget target)
        {
            target.PushTranslation(Position);
            foreach (var layer in _uiLayers)
            {
                layer.Draw(target);
            }
            target.PopTransform();
        }

        public  void Update(UiContext context, long delta)
        {
            context.PushTranslation(Position);
            foreach (var layer in _uiLayers)
            {
                layer.Update(context, delta);
            }
            context.PopTransform();
        }
    }
}
