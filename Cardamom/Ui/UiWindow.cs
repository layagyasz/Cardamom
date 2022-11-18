using SFML.Graphics;
using System.Diagnostics;

namespace Cardamom.Ui
{
    public class UiWindow
    {
        public RenderWindow RenderWindow { get; }
        public IRenderable? UiRoot { get; set; }

        private readonly UiRootController _controller;
        private readonly MouseListener _mouseListener;
        private readonly UiContext _context;

        private bool _run = true;

        public UiWindow(RenderWindow renderWindow)
        {
            RenderWindow = renderWindow;
            RenderWindow.Closed += HandleClose;

            _mouseListener = new();
            _mouseListener.Bind(renderWindow);

            _context = new(_mouseListener);

            _controller = new();
            _controller.Bind(renderWindow);
            _controller.Bind(_mouseListener);
            _controller.Bind(_context);
        }

        public void Start()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (_run)
            {
                RenderWindow.DispatchEvents();
                _context.Clear();
                RenderWindow.Clear();

                if (UiRoot != null)
                {
                    UiRoot.Update(_context, Transform.Identity, stopwatch.ElapsedMilliseconds);
                    UiRoot.Draw(RenderWindow, Transform.Identity);
                }

                _controller.DispatchEvents();
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
