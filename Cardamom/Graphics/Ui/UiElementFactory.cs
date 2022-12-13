using Cardamom.Graphics.Ui.Controller;
using Cardamom.Graphics.Ui.Elements;
using Cardamom.Window;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
{
    public class UiElementFactory
    {
        private readonly GraphicsResources _classLibrary;
        private readonly IKeyMapper _keyMapper;

        public UiElementFactory(GraphicsResources resources, IKeyMapper keyMapper)
        {
            _classLibrary = resources;
            _keyMapper = keyMapper;
        }

        public static (UiLayer, PaneLayerController) CreatePaneLayer(IEnumerable<IRenderable> panes)
        {
            var controller = new PaneLayerController();
            var layer = new UiLayer(controller);
            foreach (var pane in panes)
            {
                layer.Add(pane);
            }
            return (layer, controller);
        }

        public (UiContainer, PaneController) CreatePane(string className)
        {
            var controller = new PaneController();
            return (new UiContainer(_classLibrary.GetClass(className), controller), controller);
        }

        public (IUiElement, SelectController<T>) CreateSelect<T>(
            string className, string dropBoxClassName, IEnumerable<IUiElement> options)
        {
            var controller = new SelectController<T>("select");
            return (new Select(
                _classLibrary.GetClass(className), 
                controller,
                CreateTable(dropBoxClassName, options).Item1), controller);
        }

        public (IUiElement, SelectOptionController<T>) CreateSelectOption<T>(string className, T value, string text)
        {
            var controller = new SelectOptionController<T>(value);
            var option = new TextUiElement(_classLibrary.GetClass(className), controller);
            option.SetText(text);
            return (option, controller);
        }

        public (IUiElement, ButtonController) CreateSimpleButton(string className, Vector3 position = new())
        {
            var controller = new ButtonController();
            return 
                (new SimpleUiElement(_classLibrary.GetClass(className), controller) { Position = position },
                controller);
        }

        public (UiSerialContainer, TableController) CreateTable(
            string className, IEnumerable<IUiElement> rows, float scrollSpeed = 0, Vector3 position = new())
        {
            TableController controller = 
                scrollSpeed > 0 ? new ScrollingTableController(scrollSpeed) : new StaticTableController();
            var table = 
                new UiSerialContainer(_classLibrary.GetClass(className), controller)
                {
                    Position = position
                };
            foreach (var row in rows)
            {
                table.Add(row);
            }
            return (table, controller);
        }

        public (IUiElement, ButtonController) CreateTextButton(string className, string text, Vector3 position = new())
        {
            var controller = new ButtonController();
            var button =
                new TextUiElement(_classLibrary.GetClass(className), controller)
                {
                    Position = position
                };
            button.SetText(text);
            return (button, controller);
        }

        public (IUiElement, TextInputController) CreateTextInput(string className, Vector3 position = new())
        {
            var controller = new TextInputController("text", _keyMapper);
            return (
                new EditableTextUiElement(_classLibrary.GetClass(className), controller){ Position = position },
                controller);
        }
    }
}
