using SFML.Graphics;
using SFML.System;

namespace Cardamom.Ui
{
    public class UiContext
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

        public void Register(IUiInteractiveElement element, Transform transform)
        {
            Vector2i mousePosition = _mouseListener.GetMousePosition();
            if (element.IsPointWithinBounds(transform.GetInverse() * new Vector2f(mousePosition.X, mousePosition.Y)))
            {
                _topElement = element;
            }
        }
    }
}
