using Cardamom.Window;
using OpenTK.Windowing.Common;

namespace Cardamom.Ui.Controller.Element
{
    public class DebugController : IElementController
    {
        public EventHandler<MouseButtonClickEventArgs>? Clicked { get; set; }
        public EventHandler<EventArgs>? Focused { get; set; }
        public EventHandler<EventArgs>? FocusLeft { get; set; }
        public EventHandler<EventArgs>? MouseEntered { get; set; }
        public EventHandler<EventArgs>? MouseLeft { get; set; }


        protected object? _object;
        protected bool _focused;

        public virtual void Bind(object @object)
        {
            _object = @object;
        }

        public virtual void Unbind()
        {
            _object = default;
        }

        public bool HandleKeyDown(KeyDownEventArgs e)
        {
            PrintEvent(e);
            return true;
        }

        public bool HandleTextEntered(TextEnteredEventArgs e)
        {
            PrintEvent(e);
            return true;
        }

        public bool HandleMouseEntered()
        {
            PrintEvent("[MouseEntered]");
            MouseEntered?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public bool HandleMouseLeft()
        {
            PrintEvent("[MouseLeft]");
            MouseLeft?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public bool HandleMouseButtonClicked(MouseButtonClickEventArgs e)
        {
            PrintEvent(e);
            Clicked?.Invoke(this, e);
            return true;
        }

        public bool HandleMouseButtonDragged(MouseButtonDragEventArgs e)
        {
            PrintEvent(e);
            return true;
        }

        public bool HandleMouseWheelScrolled(MouseWheelEventArgs e)
        {
            PrintEvent(e);
            return true;
        }

        public bool HandleMouseLingered()
        {
            PrintEvent("[MouseLingered]");
            return true;
        }

        public bool HandleMouseLingerBroken()
        {
            PrintEvent("[MouseLingerBroken]");
            return true;
        }

        public bool HandleFocusEntered()
        {
            PrintEvent("[FocusEntered]");
            Focused?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public bool HandleFocusLeft()
        {
            PrintEvent("[FocusLeft]");
            FocusLeft?.Invoke(this, EventArgs.Empty);
            return true;
        }

        private void PrintEvent(object @event)
        {
            Console.WriteLine($"{_object} : {@event}");
        }
    }
}
