using Cardamom.Audio;

namespace Cardamom.Ui.Controller.Element
{
    public abstract class ClassedUiElementController<T> : UiInteractiveElementController<T> where T : ClassedUiElement
    {
        private readonly AudioPlayer? _audioPlayer;

        private bool _disable;
        private bool _hover;
        private bool _focus;
        private bool _toggle;

        public ClassedUiElementController(AudioPlayer? audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }

        public void Click()
        {
            var sound = _element!.GetAttributes(GetClassState()).SfxClick;
            if (sound != null)
            {
                _audioPlayer?.Play(sound.GetSampleProvider());
            }
        }

        public Class.State GetClassState()
        {
            return (_disable ? Class.State.Disable : Class.State.None)
                | (_hover ? Class.State.Hover : Class.State.None)
                | (_focus ? Class.State.Focus : Class.State.None)
                | (_toggle ? Class.State.Toggle : Class.State.None);
        }

        public bool GetDisable()
        {
            return _disable;
        }

        public void SetDisable(bool value)
        {
            _disable = value;
            _element!.SetState(GetClassState());
        }

        public bool GetHover()
        {
            return _hover;
        }

        public void SetHover(bool value)
        {
            _hover = value;
            _element!.SetState(GetClassState());
            if (value)
            {
                var sound = _element!.GetAttributes(GetClassState()).SfxMouseOver;
                if (sound != null)
                {
                    _audioPlayer?.Play(sound.GetSampleProvider());
                }
            }
        }

        public bool GetFocus()
        {
            return _focus;
        }

        public void SetFocus(bool value)
        {
            _focus = value;
            _element!.SetState(GetClassState());

        }

        public bool GetToggle()
        {
            return _toggle;
        }

        public void SetToggle(bool value)
        {
            _toggle = value;
            _element!.SetState(GetClassState());
        }
    }
}
