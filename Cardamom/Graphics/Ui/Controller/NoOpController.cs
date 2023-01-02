using Cardamom.Window;
using OpenTK.Windowing.Common;

namespace Cardamom.Graphics.Ui.Controller
{
    public class NoOpController<T> : IController
    {
        public EventHandler<MouseButtonClickEventArgs>? Clicked { get; set; }
        public EventHandler<EventArgs>? Focused { get; set; }


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

        public bool HandleKeyDown(KeyDownEventArgs e)
        {
            return false;
        }

        public bool HandleTextEntered(TextEnteredEventArgs e) 
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

        public bool HandleMouseButtonClicked(MouseButtonClickEventArgs e)
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
