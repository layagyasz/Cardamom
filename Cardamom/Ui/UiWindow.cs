using Cardamom.Planar;
using Cardamom.Window;
using OpenTK.Mathematics;
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

            _controller = new();
            _controller.Bind(renderWindow);
            _controller.Bind(_mouseListener);

            _context = new(_mouseListener);
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
                    UiRoot.Update(_context, Transform2.Identity, stopwatch.ElapsedMilliseconds);
                    UiRoot.Draw(RenderWindow, Transform2.Identity);
                }

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
