using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardamom.Ui
{
    public class UiWindow
    {
        public RenderWindow RenderWindow { get; }
        public IRenderable? UiRoot { get; set; }

        public UiWindow(RenderWindow renderWindow)
        {
            this.RenderWindow = renderWindow;
        }

        public void Start()
        {
            UiContext context = new UiContext();
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                RenderWindow.DispatchEvents();

                if (UiRoot != null)
                {
                    UiRoot.Update(context, Transform.Identity, stopwatch.ElapsedMilliseconds);
                    UiRoot.Draw(RenderWindow, Transform.Identity);
                }

                RenderWindow.Display();
            }
        }
    }
}
