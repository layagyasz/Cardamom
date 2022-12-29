using Cardamom.Window;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
{
    public class UiContext : GraphicsContext
    {
        private MouseListener? _mouseListener;

        private IUiInteractiveElement? _topElement;
        private float _topD;

        public UiContext(Box2i viewPort)
            : base(viewPort) { }

        public void Bind(MouseListener mouseListener)
        {
            _mouseListener = mouseListener;
        }

        public override void Clear()
        {
            _topElement = null;
            _topD = float.MaxValue;
        }

        public override void Flatten()
        {
            _topD = float.MaxValue;
        }

        public IUiInteractiveElement? GetTopElement()
        {
            return _topElement;
        }

        public void Register(IUiInteractiveElement element)
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
                    var dz = new Vector3(inverted.Row2.X, inverted.Row2.Y, inverted.Row2.Z);
                    float? d = element.GetRayIntersection(new(origin, dz));
                    if (d != null && d <= _topD)
                    {
                        _topElement = element;
                        _topD = d.Value;
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
