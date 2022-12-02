using Cardamom.Graphics.Ui.Controller;
using Cardamom.Graphics.Ui.Elements;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
{
    public class UiElementFactory
    {
        private readonly ClassLibrary _classLibrary;

        public UiElementFactory(ClassLibrary classLibrary)
        {
            _classLibrary = classLibrary;
        }

        public static UiLayer CreatePaneLayer(IEnumerable<IRenderable> panes)
        {
            var layer = new UiLayer(new PaneLayerController());
            foreach (var pane in panes)
            {
                layer.Add(pane);
            }
            return layer;
        }

        public UiContainer CreatePane(string className)
        {
            return new UiContainer(_classLibrary.GetClass(className), new PaneController());
        }

        public IUiElement CreateSimpleButton(string className, Vector2 position = new())
        {
            return new SimpleUiElement(_classLibrary.GetClass(className), new ButtonController()) 
            { 
                Position = position
            };
        }

        public IUiElement CreateTextButton(string className, string text, Vector2 position = new())
        {
            var button =
                new TextUiElement(_classLibrary.GetClass(className), new ButtonController())
                {
                    Position = position
                };
            button.SetText(text);
            return button;
        }
    }
}
