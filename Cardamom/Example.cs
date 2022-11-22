using Cardamom.Ui;
using Cardamom.Ui.Controller;
using Cardamom.Ui.Elements;
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
                                    .ReadFonts("Example/Fonts.json")
                                    .ReadClasses("Example", "Style.json")
                                    .Build());
            var pane = uiElementFactory.CreatePane("example-base-class");
            pane.Add(uiElementFactory.CreateSimpleButton("example-child-class", new(100, 100)));
            var screen = 
                new Screen(
                    new Planar.Rectangle(new(), new(800, 600)), 
                    new SecondaryController<Screen>(),
                    new List<UiLayer>()
                    {
                        new UiLayer(new SecondaryController<UiLayer>())
                        {
                            pane
                        }
                    });
            ui.UiRoot = screen;
            ui.Start();
        }
    }
}
