﻿using Cardamom.Window;
using OpenTK.Windowing.Common;

namespace Cardamom.Ui.Controller.Element
{
    public abstract class UiInteractiveElementController<T> : IElementController where T : IInteractive
    {
        public EventHandler<MouseButtonClickEventArgs>? Clicked { get; set; }
        public EventHandler<EventArgs>? Focused { get; set; }
        public EventHandler<MouseButtonDragEventArgs>? MouseDragged { get; set; }
        public EventHandler<EventArgs>? FocusLeft { get; set; }
        public EventHandler<EventArgs>? MouseEntered { get; set; }
        public EventHandler<EventArgs>? MouseLeft { get; set; }

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

        public abstract bool HandleMouseButtonClicked(MouseButtonClickEventArgs e);

        public virtual bool HandleMouseButtonDragged(MouseButtonDragEventArgs e)
        {
            return false;
        }

        public virtual bool HandleMouseWheelScrolled(MouseWheelEventArgs e)
        {
            return false;
        }

        public virtual bool HandleMouseLingered()
        {
            return false;
        }

        public virtual bool HandleMouseLingerBroken()
        {
            return false;
        }

        public virtual bool HandleFocusEntered()
        {
            return false;
        }

        public virtual bool HandleFocusLeft()
        {
            return false;
        }
    }
}
