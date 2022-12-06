using Cardamom.Window;

namespace Cardamom.Graphics.Ui
{
    public class UiContext : GraphicsContext
    {
        private readonly MouseListener _mouseListener;

        private IUiInteractiveElement? _topElement;

        public UiContext(MouseListener mouseListener)
        {
            _mouseListener = mouseListener;
        }

        public void Clear()
        {
            _topElement = null;
        }

        public IUiInteractiveElement? GetTopElement()
        {
            return _topElement;
        }

        public void Register(IUiInteractiveElement element)
        {
            if (GetScissor() == null || GetScissor()!.Value.Contains(_mouseListener.GetMousePosition()))
            {
                if (element.IsPointWithinBounds(GetTransform().GetInverse() * _mouseListener.GetMousePosition()))
                {
                    _topElement = element;
                }
            }
        }
    }
}
