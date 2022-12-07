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
            long elapsed = 0;
            while (_run)
            {
                RenderWindow.DispatchEvents();
                RenderWindow.Clear();
                _context.Clear();

                long frameElapsed = stopwatch.ElapsedMilliseconds;
                if (UiRoot != null)
                {
                    UiRoot.Update(_context, frameElapsed - elapsed);
                    UiRoot.Draw(RenderWindow);
                }
                elapsed = frameElapsed;

                _controller.DispatchEvents();
                RenderWindow.Display();
            }
        }

        private void HandleClose(object? sender, EventArgs e)
        {
            _run = false;
        }
    }
}
