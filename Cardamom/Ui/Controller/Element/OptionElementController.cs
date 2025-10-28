using Cardamom.Audio;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Ui.Controller.Element
{
    public class OptionElementController<T> : SimpleElementController, IOptionController<T>
    {
        public EventHandler<EventArgs>? Selected { get; set; }

        public T Key { get; }

        public OptionElementController(AudioPlayer? audioPlayer, T key)
            : base(audioPlayer)
        {
            Key = key;
        }

        public void SetSelected(bool selected)
        {
            SetToggle(selected);
        }

        public override bool HandleMouseButtonClicked(MouseButtonClickEventArgs e)
        {
            base.HandleMouseButtonClicked(e);
            if (e.Button == MouseButton.Left)
            {
                Selected?.Invoke(this, EventArgs.Empty);
            }
            return true;
        }
    }
}
