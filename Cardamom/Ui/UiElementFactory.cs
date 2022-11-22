using Cardamom.Ui.Controller;
using Cardamom.Ui.Elements;
using OpenTK.Mathematics;

namespace Cardamom.Ui
{
    public class UiElementFactory
    {
        private readonly ClassLibrary _classLibrary;

        public UiElementFactory(ClassLibrary classLibrary)
        {
            _classLibrary = classLibrary;
        }

        public UiContainer CreatePane(string className)
        {
            return new UiContainer(_classLibrary.Get(className), new PaneController());
        }

        public IUiElement CreateSimpleButton(string className, Vector2 position = new())
        {
            return new SimpleUiElement(_classLibrary.Get(className), new ButtonController()) { Position = position };
        }
    }
}
