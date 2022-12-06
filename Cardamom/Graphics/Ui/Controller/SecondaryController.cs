using Cardamom.Window;
using OpenTK.Windowing.Common;

namespace Cardamom.Graphics.Ui.Controller
{
    public class SecondaryController<T> : IController
    {

        protected T? _element;

        public virtual void Bind(object @object)
        {
            _element = (T)@object;
        }

        public virtual void Unbind()
        {
            _element = default;
        }

        public T GetElement()
        {
            return _element!;
        }

        public bool HandleKeyPressed(KeyboardKeyEventArgs e) 
        {
            return false;
        }

        public bool HandleMouseEntered() 
        {
            return false;
        }

        public bool HandleMouseLeft()
        {
            return false;
        }

        public bool HandleMouseButtonClicked(MouseButtonEventArgs e)
        {
            return false;
        }

        public bool HandleMouseButtonDragged(MouseButtonDragEventArgs e)
        {
            return false;
        }

        public bool HandleMouseWheelScrolled(MouseWheelEventArgs e) 
        {
            return false;
        }

        public bool HandleFocusEntered() 
        {
            return false;
        }

        public bool HandleFocusLeft()
        {
            return false;
        }
    }
}
