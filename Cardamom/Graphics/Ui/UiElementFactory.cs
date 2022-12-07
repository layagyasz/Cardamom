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

        public IUiElement CreateSelect<T>(string className, string dropBoxClassName, IEnumerable<IUiElement> options)
        {
            return new Select(
                _classLibrary.GetClass(className), 
                new SelectController<T>("select"),
                CreateTable(dropBoxClassName, options));
        }

        public IUiElement CreateSelectOption<T>(string className, T value, string text)
        {
            var option = new TextUiElement(_classLibrary.GetClass(className), new SelectOptionController<T>(value));
            option.SetText(text);
            return option;
        }

        public IUiElement CreateSimpleButton(string className, Vector2 position = new())
        {
            return new SimpleUiElement(_classLibrary.GetClass(className), new ButtonController()) 
            { 
                Position = position
            };
        }

        public UiSerialContainer CreateTable(
            string className, IEnumerable<IUiElement> rows, float scrollSpeed = 0, Vector2 position = new())
        {
            var table =
                new UiSerialContainer(
                    _classLibrary.GetClass(className),
                    scrollSpeed > 0 ? new ScrollingTableController(scrollSpeed) : new StaticTableController())
                {
                    Position = position
                };
            foreach (var row in rows)
            {
                table.Add(row);
            }
            return table;
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
