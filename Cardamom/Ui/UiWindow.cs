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
        public UiRootController UiRootController { get; }
        public MouseListener MouseListener { get; }
        public IRenderable? UiRoot { get; set; }

        private readonly UiContext _context;

        private bool _run = true;

        public UiWindow(RenderWindow renderWindow)
        {
            RenderWindow = renderWindow;
            RenderWindow.Closed += HandleClose;
            MouseListener = new MouseListener(renderWindow);
            UiRootController = new UiRootController(RenderWindow, MouseListener);
            _context = new UiContext(MouseListener);
        }

        public void Start()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (_run)
            {
                RenderWindow.DispatchEvents();

                if (UiRoot != null)
                {
                    UiRoot.Update(_context, Transform.Identity, stopwatch.ElapsedMilliseconds);
                    UiRoot.Draw(RenderWindow, Transform.Identity);
                }

                RenderWindow.Display();
            }
        }

        private void HandleClose(object? sender, EventArgs e)
        {
            RenderWindow.Close();
            _run = false;
        }
    }
}
