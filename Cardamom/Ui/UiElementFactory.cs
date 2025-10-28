using Cardamom.Audio;
using Cardamom.Graphics;
using Cardamom.Graphics.TexturePacking;
using Cardamom.Ui.Controller;
using Cardamom.Ui.Controller.Element;
using Cardamom.Ui.Elements;
using OpenTK.Mathematics;

namespace Cardamom.Ui
{
    public class UiElementFactory
    {
        private readonly AudioPlayer _audioPlayer;
        private readonly GameResources _resources;

        public UiElementFactory(AudioPlayer audioPlayer, GameResources resources)
        {
            _audioPlayer = audioPlayer;
            _resources = resources;
        }

        public static (UiGroup, PaneLayerController) CreatePaneLayer(IEnumerable<IUiElement> panes)
        {
            var controller = new PaneLayerController();
            var layer = new UiGroup(controller);
            foreach (var pane in panes)
            {
                layer.Add(pane);
            }
            return (layer, controller);
        }

        public (UiContainer, PaneController) CreatePane(string className)
        {
            var controller = new PaneController(_audioPlayer);
            return (new UiContainer(_resources.GetClass(className), controller), controller);
        }

        public (IUiComponent, RadioController<T>) CreateRadio<T>(
            Radio.Style style,
            IEnumerable<SelectOption<T>> range,
            bool isNullable = false, 
            T? initialValue = default,
            float scrollSpeed = 0)
        {
            var controller = new RadioController<T>(isNullable, initialValue);
            return (
                new Radio(
                    controller,
                    CreateTable(
                        style.Container!,
                        range.Select(
                            x => new TextUiElement(
                                _resources.GetClass(style.Option!),
                                new OptionElementController<T>(_audioPlayer, x.Value),
                                x.Text))
                        .ToList(), 
                        scrollSpeed).Item1),
                        controller);
        }

        public (IUiComponent, SelectController<T>) CreateSelect<T>(
            Select.Style style,
            IEnumerable<SelectOption<T>> range,
            T? initialValue = default,
            float scrollSpeed = 0)
        {
            var controller = new SelectController<T>(range, initialValue);
            return (
                new Select(
                    controller,
                    new TextUiElement(
                        _resources.GetClass(style.Root!), new RootElementController(_audioPlayer), string.Empty),
                    new UiCompoundComponent(
                        new RadioController<T>(), 
                        CreateTable(style.OptionContainer!, new List<IUiElement>(), scrollSpeed).Item1),
                    _resources.GetClass(style.Option!),
                    _audioPlayer),
                controller);
        }

        public (IUiElement, SimpleElementController) CreateSimpleButton(string className, Vector3 position = new())
        {
            var controller = new SimpleElementController(_audioPlayer);
            return
                (new SimpleUiElement(_resources.GetClass(className), controller) { Position = position },
                controller);
        }

        public (UiSerialContainer, TableController) CreateTable(
            string className, IEnumerable<IUiElement> rows, float scrollSpeed = 0, Vector3 position = new())
        {
            var controller = new TableController(_audioPlayer, scrollSpeed);
            var table =
                new UiSerialContainer(
                    _resources.GetClass(className), 
                    controller, 
                    UiSerialContainer.Orientation.Vertical)
                {
                    Position = position
                };
            foreach (var row in rows)
            {
                table.Add(row);
            }
            return (table, controller);
        }

        public UiSerialContainer CreateTableRow(
            string className, IEnumerable<IUiElement> elements, IElementController controller)
        {
            var row =
                new UiSerialContainer(
                    _resources.GetClass(className), controller, UiSerialContainer.Orientation.Horizontal);
            foreach (var element in elements)
            {
                row.Add(element);
            }
            return row;
        }

        public (IUiElement, SimpleElementController) CreateTextButton(string className, string text, Vector3 position = new())
        {
            var controller = new SimpleElementController(_audioPlayer);
            var button =
                new TextUiElement(_resources.GetClass(className), controller, text)
                {
                    Position = position
                };
            return (button, controller);
        }

        public (IUiElement, TextInputController) CreateTextInput(string className, Vector3 position = new())
        {
            var controller = new TextInputController(_audioPlayer);
            return (
                new EditableTextUiElement(
                    _resources.GetClass(className), controller, string.Empty) { Position = position },
                controller);
        }

        public AudioPlayer GetAudioPlayer()
        {
            return _audioPlayer;
        }

        public Class GetClass(string key)
        {
            return _resources.GetClass(key);
        }

        public RenderShader GetShader(string key)
        {
            return _resources.GetShader(key);
        }

        public TextureSegment GetTexture(string key)
        {
            return _resources.GetTexture(key);
        }
    }
}
