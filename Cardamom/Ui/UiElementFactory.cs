using Cardamom.Ui.Controller;
using Cardamom.Ui.Elements;
using SFML.System;

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

        public IUiElement CreateSimpleButton(string className, Vector2f position = new())
        {
            return new SimpleUiElement(_classLibrary.Get(className), new ButtonController()) { Position = position };
        }
    }
}
