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
            return new SimpleUiElement(_classLibrary.GetClass(className), new ButtonController()) { Position = position };
        }
    }
}
