using Cardamom.Window;
using OpenTK.Windowing.Common;

namespace Cardamom.Graphics.Ui.Controller
{
    public abstract class UiInteractiveElementController<T> : IController where T : IUiInteractiveElement
    {
        public EventHandler<MouseButtonEventArgs>? Clicked { get; set; }
        public EventHandler<EventArgs>? Focused { get; set; }

        protected T? _element;

        public virtual void Bind(object @object)
        {
            _element = (T)@object;
        }

        public void Unbind()
        {
            _element = default;
        }

        public T GetElement()
        {
            return _element!;
        }

        public virtual bool HandleKeyDown(KeyDownEventArgs e)
        {
            return false;
        }

        public virtual bool HandleTextEntered(TextEnteredEventArgs e)
        {
            return false;
        }

        public virtual bool HandleMouseEntered() 
        {
            return false;
        }

        public virtual bool HandleMouseLeft() 
        {
            return false;
        }

        public virtual bool HandleMouseButtonClicked(MouseButtonEventArgs e)
        {
            Clicked?.Invoke(this, e);
            return true;
        }

        public virtual bool HandleMouseButtonDragged(MouseButtonDragEventArgs e) 
        {
            return false;
        }

        public virtual bool HandleMouseWheelScrolled(MouseWheelEventArgs e) 
        {
            return false;
        }

        public virtual bool HandleFocusEntered()
        {
            Focused?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public virtual bool HandleFocusLeft() 
        {
            return false;
        }
    }
}
