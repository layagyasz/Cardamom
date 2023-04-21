using Cardamom.Graphics;
using Cardamom.Mathematics.Geometry;
using Cardamom.Window;
using OpenTK.Mathematics;

namespace Cardamom.Ui
{
    public class SimpleUiContext : IUiContext
    {
        private MouseListener? _mouseListener;
        private GraphicsContext? _context;

        private IInteractive? _topElement;
        private float _topDistance;
        private Vector3 _topIntersection;

        public void Bind(MouseListener mouseListener)
        {
            _mouseListener = mouseListener;
        }

        public void Bind(GraphicsContext context)
        {
            _context = context;
        }

        public void Clear()
        {
            _topElement = null;
            _topDistance = float.MaxValue;
            _topIntersection = Vector3.Zero;
        }

        public void Flatten()
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
            if (_mouseListener != null && _context != null)
            {
                var ndcMouse = WindowToNdc(_mouseListener.GetMousePosition());
                var projection = _context.GetProjection();
                var frustrumMouse = new Vector4(ndcMouse.X, ndcMouse.Y, projection.NearPlane, 1);
                var projectedMouse = frustrumMouse * projection.Matrix.Inverted();
                var scissor = _context!.GetScissor();
                if (scissor == null || scissor.Value.ContainsInclusive(projectedMouse.Xy))
                {
                    var transform = _context.GetModelMatrix() * _context.GetViewMatrix() * projection.Matrix;
                    var inverted = transform.Inverted();
                    var worldMouse = frustrumMouse * inverted;
                    var front =
                        worldMouse + new Vector4(inverted.Row2.X, inverted.Row2.Y, inverted.Row2.Z, inverted.Row2.W);
                    worldMouse /= worldMouse.W;
                    var dz = worldMouse - front / front.W;
                    var ray = new Ray3(worldMouse.Xyz, dz.Xyz);
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

        public Vector2 NdcToWindow(Vector2 position)
        {
            return new(
                _context!.GetViewPort().HalfSize.X * (position.X + 1),
                _context.GetViewPort().HalfSize.Y * (1 - position.Y));
        }

        public Vector2 WindowToNdc(Vector2 position)
        {
            return new(
                position.X / _context!.GetViewPort().HalfSize.X - 1,
                1 - position.Y / _context!.GetViewPort().HalfSize.Y);
        }
    }
}
