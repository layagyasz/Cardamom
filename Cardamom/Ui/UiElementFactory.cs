using Cardamom.Ui.Controller;
using Cardamom.Ui.Elements;

namespace Cardamom.Ui
{
    public class UiElementFactory
    {
        private readonly ClassLibrary _classLibrary;

        public UiElementFactory(ClassLibrary classLibrary)
        {
            _classLibrary = classLibrary;
        }

        public IUiElement CreatePane(string className)
        {
            return new UiContainer(_classLibrary.Get(className), new PaneController());
        }

        public IUiElement CreateSimpleButton(string className)
        {
            return new SimpleUiElement(_classLibrary.Get(className), new ButtonController());
        }
    }
}
