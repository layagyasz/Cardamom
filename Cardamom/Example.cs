using Cardamom.Ui;
using Cardamom.Ui.Controller;
using Cardamom.Ui.Elements;
using SFML.Graphics;
using SFML.Window;

namespace Cardamom
{
    public static class Example
    {
        public static void Main(string[] args)
        {
            var window = new RenderWindow(new VideoMode(800, 600), "Cardamom");
            var ui = new UiWindow(window);
            var uiElementFactory = 
                new UiElementFactory(
                    new ClassLibrary.Builder()
                                    .ReadFonts("Example/Fonts.json")
                                    .ReadClasses("Example", "Style.json")
                                    .Build());
            var pane = uiElementFactory.CreatePane("example-base-class");
            pane.Add(uiElementFactory.CreateSimpleButton("example-base-class", new(100, 100)));
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
