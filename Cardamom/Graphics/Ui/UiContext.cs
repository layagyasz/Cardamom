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
                var mouse = new Vector4(ndcMouse.X, ndcMouse.Y, 0, 1) * GetProjectionMatrix().Inverted();
                if (GetScissor() == null 
                    || GetScissor()!.Value.ContainsInclusive(mouse.Xy))
                {
                    var inverted = GetViewMatrix().Inverted();
                    var origin =
                        new Vector3(
                            inverted.Row0.X * mouse.X + inverted.Row1.X * mouse.Y + inverted.Row3.X,
                            inverted.Row0.Y * mouse.X + inverted.Row1.Y * mouse.Y + inverted.Row3.Y,
                            inverted.Row0.Z * mouse.X + inverted.Row1.Z * mouse.Y + inverted.Row3.Z);
                    var dz = -new Vector3(inverted.Row2.X, inverted.Row2.Y, inverted.Row2.Z);
                    float? d = element.GetRayIntersection(new(origin, dz));
                    if (d != null && !(d < 0) && d <= _topDistance)
                    {
                        _topElement = element;
                        _topDistance = d.Value;
                        _topIntersection = origin + dz * d.Value;
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
