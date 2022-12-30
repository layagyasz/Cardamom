using Cardamom.Graphics.Ui.Elements;
using Cardamom.Mathematics.Geometry;
using Cardamom.Window;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
{
    public class UiContext : GraphicsContext
    {
        private MouseListener? _mouseListener;

        private IInteractive? _topElement;
        private float _topDistance;
        private Vector3 _topIntersection;

        public UiContext(Box2i viewPort)
            : base(viewPort) { }

        public void Bind(MouseListener mouseListener)
        {
            _mouseListener = mouseListener;
        }

        public override void Clear()
        {
            _topElement = null;
            _topDistance = float.MaxValue;
            _topIntersection = Vector3.Zero;
        }

        public override void Flatten()
        {
            _topDistance = float.MaxValue;
        }

        public IInteractive? GetTopElement()
        {
            return _topElement;
        }

        public Vector3 GetTopIntersection()
        {
            return _topIntersection;
        }

        public void Register(IInteractive element)
        {
            if (_mouseListener != null)
            {
                var ndcMouse = WindowToNdc(_mouseListener.GetMousePosition());
                var projection = GetProjection();
                var windowMouse = new Vector4(ndcMouse.X, ndcMouse.Y, projection.NearPlane, 1);
                var projectedMouse = windowMouse * projection.Matrix.Inverted();
                if (GetScissor() == null 
                    || GetScissor()!.Value.ContainsInclusive(projectedMouse.Xy))
                {
                    var transform = GetViewMatrix() * projection.Matrix;
                    var inverted = transform.Inverted();
                    var origin = windowMouse * inverted;
                    origin /= origin.W;
                    var dz = (windowMouse + new Vector4(0, 0, 1, 0)) * inverted;
                    dz = origin - dz / dz.W;
                    var ray = new Ray3(origin.Xyz, dz.Xyz);
                    float? d = element.GetRayIntersection(ray);
                    if (d != null && !(d < 0) && d <= _topDistance)
                    {
                        _topElement = element;
                        _topDistance = d.Value;
                        _topIntersection = ray.Point + ray.Direction * d.Value;
                    }
                }
            }
        }

        private Vector2 WindowToNdc(Vector2 position)
        {
            return new(
                position.X / GetViewPort().HalfSize.X - 1,
                1 - position.Y / GetViewPort().HalfSize.Y);
        }
    }
}
