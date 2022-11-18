using Cardamom.Planar;
using Cardamom.Ui.Controller;
using SFML.Graphics;
using SFML.System;

namespace Cardamom.Ui.Elements
{
    public class Screen : IUiInteractiveElement
    {
        public bool Visible { get; set; } = true;
        public Vector2f Size => _bounds.Size;
        public Vector2f Position { get; set; }
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

        public bool IsPointWithinBounds(Vector2f point)
        {
            return _bounds.ContainsPoint(point);
        }

        public void Draw(RenderTarget target, Transform transform)
        {
            transform.Translate(Position);
            foreach (var layer in _uiLayers)
            {
                layer.Draw(target, transform);
            }
        }

        public  void Update(UiContext context, Transform transform, long delta)
        {
            transform.Translate(Position);
            foreach (var layer in _uiLayers)
            {
                layer.Update(context, transform, delta);
            }
        }
    }
}
