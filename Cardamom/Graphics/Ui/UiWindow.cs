using Cardamom.Planar;
using Cardamom.Window;
using System.Diagnostics;

namespace Cardamom.Graphics.Ui
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
                RenderWindow.Clear();
                _context.Clear();

                if (UiRoot != null)
                {
                    UiRoot.Update(_context, stopwatch.ElapsedMilliseconds);
                    UiRoot.Draw(RenderWindow);
                }

                _controller.DispatchEvents();
                RenderWindow.Display();
                stopwatch.Restart();
            }
        }

        private void HandleClose(object? sender, EventArgs e)
        {
            _run = false;
        }
    }
}
