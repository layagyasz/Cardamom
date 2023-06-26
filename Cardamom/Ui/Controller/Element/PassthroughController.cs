using Cardamom.Window;
using OpenTK.Windowing.Common;

namespace Cardamom.Ui.Controller.Element
{
    public class PassthroughController : IElementController
    {
        public EventHandler<MouseButtonClickEventArgs>? Clicked { get; set; }
        public EventHandler<EventArgs>? Focused { get; set; }
        public EventHandler<EventArgs>? FocusLeft { get; set; }
        public EventHandler<EventArgs>? MouseEntered { get; set; }
        public EventHandler<MouseButtonDragEventArgs>? MouseDragged { get; set; }
        public EventHandler<EventArgs>? MouseLeft { get; set; }

        private object? _object;
        private IElementController? _subcontroller;

        public PassthroughController() { }

        public PassthroughController(IElementController subcontroller)
        {
            _subcontroller = subcontroller;
        }

        public void SetSubcontroller(IElementController subcontroller)
        {
            _subcontroller = subcontroller;
            _subcontroller.Clicked += Clicked;
            _subcontroller.Focused += Focused;
            _subcontroller.FocusLeft += FocusLeft;
            _subcontroller.MouseEntered += MouseEntered;
            _subcontroller.MouseLeft += MouseLeft;
        }

        public virtual void Bind(object @object)
        {
            _object = @object;
        }

        public virtual void Unbind()
        {
            _object = default;
            if (_subcontroller != null)
            {
                _subcontroller.Unbind();
                _subcontroller.Clicked -= Clicked;
                _subcontroller.Focused -= Focused;
                _subcontroller.FocusLeft -= FocusLeft;
                _subcontroller.MouseEntered -= MouseEntered;
                _subcontroller.MouseLeft -= MouseLeft;
            }
        }

        public bool HandleKeyDown(KeyDownEventArgs e)
        {
            return _subcontroller!.HandleKeyDown(e);
        }

        public bool HandleTextEntered(TextEnteredEventArgs e)
        {
            return _subcontroller!.HandleTextEntered(e);
        }

        public bool HandleMouseEntered()
        {
            return _subcontroller!.HandleMouseEntered();
        }

        public bool HandleMouseLeft()
        {
            return _subcontroller!.HandleMouseLeft();
        }

        public bool HandleMouseButtonClicked(MouseButtonClickEventArgs e)
        {
            return _subcontroller!.HandleMouseButtonClicked(e);
        }

        public bool HandleMouseButtonDragged(MouseButtonDragEventArgs e)
        {
            return _subcontroller!.HandleMouseButtonDragged(e);
        }

        public bool HandleMouseWheelScrolled(MouseWheelEventArgs e)
        {
            return _subcontroller!.HandleMouseWheelScrolled(e);
        }

        public bool HandleMouseLingered()
        {
            return _subcontroller!.HandleMouseLingered();
        }

        public bool HandleMouseLingerBroken()
        {
            return _subcontroller!.HandleMouseLingerBroken();
        }

        public bool HandleFocusEntered()
        {
            return _subcontroller!.HandleFocusEntered();
        }

        public bool HandleFocusLeft()
        {
            return _subcontroller!.HandleFocusLeft();
        }
    }
}
