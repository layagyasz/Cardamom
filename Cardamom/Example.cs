using Cardamom.Ui;
using SFML.Graphics;
using SFML.Window;

namespace Cardamom
{
    public static class Example
    {
        public static void Main(string[] args)
        {
            var window = new RenderWindow(new VideoMode(400, 300), "Cardamom");
            var ui = new UiWindow(window);
            ui.Start();
        }
    }
}
