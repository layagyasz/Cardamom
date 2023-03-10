using Cardamom.Ui.Elements;

namespace Cardamom.Ui.Controller.Element
{
    public class SelectOptionElementController<T> : OptionElementController<T>
    {
        public SelectOptionElementController(T key)
            : base(key) { }

        public string GetText()
        {
            return ((TextUiElement)_element!).GetText();
        }
    }
}
