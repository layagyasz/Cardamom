namespace Cardamom.Ui.Controller
{
    public abstract class ClassedUiElementController<T> : UiInteractiveElementController<T> where T : ClassedUiElement
    {
        private bool _disable;
        private bool _hover;
        private bool _focus;
        private bool _toggle;

        public Class.State GetClassState()
        {
            return (_disable ? Class.State.DISABLE : Class.State.NONE)
                | (_hover ? Class.State.HOVER : Class.State.NONE)
                | (_focus ? Class.State.FOCUS : Class.State.NONE)
                | (_toggle ? Class.State.TOGGLE : Class.State.NONE);
        }

        public void SetDisable(bool value)
        {
            _disable = value;
            _element?.SetState(GetClassState());
        }

        public void SetHover(bool value)
        {
            _hover = value;
            _element?.SetState(GetClassState());

        }

        public void SetFocus(bool value)
        {
            _focus = value;
            _element?.SetState(GetClassState());

        }

        public void SetToggle(bool value)
        {
            _toggle = value;
            _element?.SetState(GetClassState());
        }
    }
}
