using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Ui.Controller.Element
{
    public class OptionElementController<T> : ClassedUiElementController<ClassedUiElement>, IOptionController<T>
    {
        public EventHandler<EventArgs>? Selected { get; set; }

        public T Key { get; }

        public OptionElementController(T key)
        {
            Key = key;
        }

        public void SetSelected(bool selected)
        {
            SetToggle(selected);
        }

        public override bool HandleMouseButtonClicked(MouseButtonClickEventArgs e)
        {
            Clicked?.Invoke(this, e);
            if (e.Button == MouseButton.Left)
            {
                Selected?.Invoke(this, EventArgs.Empty);
            }
            return true;
        }

        public override bool HandleMouseEntered()
        {
            SetHover(true);
            return true;
        }

        public override bool HandleMouseLeft()
        {
            SetHover(false);
            return true;
        }
    }
}
