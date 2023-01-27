using Cardamom.Graphics.Ui.Controller;
using Cardamom.Mathematics.Geometry;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements
{
    public class Screen : IInteractive, IUiElement
    {
        public bool Visible { get; set; } = true;
        public Vector3 Size => new(_bounds.Size.X, _bounds.Size.Y, 0);
        public Vector3 Position { get; set; }
        public IController Controller { get; set; }
        public IControlled? Parent { get; set; }

        protected readonly IUiLayer[] _uiLayers;

        protected readonly Rectangle _bounds;

        public Screen(Rectangle bounds, IController controller, IEnumerable<IUiLayer> uiLayers)
        {
            Controller = controller;
            _bounds = bounds;
            _uiLayers = uiLayers.ToArray();
        }

        public virtual void Initialize()
        {
            foreach (var layer in _uiLayers)
            {
                layer.Initialize();
            }
            Controller.Bind(this);
        }

        public virtual void ResizeContext(Vector3 bounds) { }

        public float? GetRayIntersection(Ray3 ray)
        {
            return float.MaxValue;
        }

        public virtual void Draw(RenderTarget target, UiContext context)
        {
            context.Register(this);
            target.PushTranslation(Position);
            foreach (var layer in _uiLayers)
            {
                layer.Draw(target, context);
            }
            target.PopModelMatrix();
        }

        public  virtual void Update(long delta)
        {
            foreach (var layer in _uiLayers)
            {
                layer.Update(delta);
            }
        }
    }
}
