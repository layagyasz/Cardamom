using Cardamom.Mathematics.Geometry;
using Cardamom.Window;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
{
    public class UiContext : GraphicsContext
    {
        private readonly MouseListener _mouseListener;

        private IUiInteractiveElement? _topElement;
        private float _topZ;

        public UiContext(Box2i viewPort, MouseListener mouseListener)
            : base(viewPort)
        {
            _mouseListener = mouseListener;
        }

        public override void Clear()
        {
            _topElement = null;
            _topZ = float.MinValue;
        }

        public override void Flatten()
        {
            _topZ = float.MinValue;
        }

        public IUiInteractiveElement? GetTopElement()
        {
            return _topElement;
        }

        public void Register(IUiInteractiveElement element)
        {
            if (_topElement == null || element.Position.Z >= _topZ)
            {
                if (GetScissor() == null || GetScissor()!.Value.ContainsInclusive(_mouseListener.GetMousePosition()))
                {
                    var inverted = GetViewMatrix().Inverted();
                    var mouse = _mouseListener.GetMousePosition();
                    var origin =
                        new Vector3(
                            inverted.Row0.X * mouse.X + inverted.Row1.X * mouse.Y + inverted.Row3.X,
                            inverted.Row0.Y * mouse.X + inverted.Row1.Y * mouse.Y + inverted.Row3.Y,
                            inverted.Row0.Z * mouse.X + inverted.Row1.Z * mouse.Y + inverted.Row3.Z);
                    var dz = new Vector3(inverted.Row2.X, inverted.Row2.Y, inverted.Row2.Z);
                    if (element.IntersectsRay(origin, dz))
                    {
                        _topElement = element;
                        _topZ = element.Position.Z;
                    }
                }
            }
        }
    }
}
