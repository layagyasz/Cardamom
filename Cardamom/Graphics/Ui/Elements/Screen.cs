using Cardamom.Graphics.Ui.Controller;
using Cardamom.Planar;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements
{
    public class Screen : IUiInteractiveElement
    {
        public bool Visible { get; set; } = true;
        public Vector2 Size => _bounds.Size;
        public Vector2 Position { get; set; }
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

        public bool IsPointWithinBounds(Vector2 point)
        {
            return _bounds.ContainsPoint(point);
        }

        public void Draw(RenderTarget target, Transform2 transform)
        {
            transform.Translate(Position);
            foreach (var layer in _uiLayers)
            {
                layer.Draw(target, transform);
            }
        }

        public  void Update(UiContext context, Transform2 transform, long delta)
        {
            transform.Translate(Position);
            foreach (var layer in _uiLayers)
            {
                layer.Update(context, transform, delta);
            }
        }
    }
}
