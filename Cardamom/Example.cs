using Cardamom.Graphics;
using Cardamom.Graphics.Ui;
using Cardamom.Graphics.Ui.Controller;
using Cardamom.Graphics.Ui.Elements;
using Cardamom.Window;
using OpenTK.Mathematics;

namespace Cardamom
{
    public static class Example
    {
        public static void Main()
        {
            var window = new RenderWindow("Cardamom - Example", new Vector2i(800, 600));
            var ui = new UiWindow(window);
            var uiElementFactory =
                new UiElementFactory(
                    new ClassLibrary.Builder()
                                    .ReadTextures("Example/Textures.json")
                                    .ReadFonts("Example/Fonts.json")
                                    .ReadShaders("Example/Shaders.json")
                                    .ReadClasses("Example", "Style.json")
                                    .Build());
            var pane = uiElementFactory.CreatePane("example-base-class").Item1;
            var options = new List<IUiElement>();
            for (int i = 0; i < 20; ++i)
            {
                options.Add(
                    uiElementFactory.CreateSelectOption("example-select-option-class", i, $"Button #{i}").Item1);
            }
            var select = 
                uiElementFactory.CreateSelect<int>("example-select-class", "example-select-drop-box-class", options);
            select.Item2.ValueChanged += (s, e) => Console.WriteLine(e);
            pane.Add(select.Item1);
            var screen = 
                new Screen(
                    new Planar.Rectangle(new(), new(800, 600)), 
                    new SecondaryController<Screen>(),
                    new List<UiLayer>()
                    {
                        UiElementFactory.CreatePaneLayer(new List<IRenderable>() { pane }).Item1
                    });
            screen.Initialize();
            ui.UiRoot = screen;
            ui.Start();
        }
    }
}
