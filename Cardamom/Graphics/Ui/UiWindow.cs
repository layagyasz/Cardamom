using Cardamom.Window;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System.Diagnostics;

namespace Cardamom.Graphics.Ui
{
    public class UiWindow
    {
        public RenderWindow RenderWindow { get; }
        public IRenderable? UiRoot { get; set; }

        private readonly UiRootController _controller;
        private readonly UiContext _context;

        private MouseListener? _mouseListener;
        private KeyboardListener? _keyboardListener;

        private bool _run = true;

        public UiWindow(RenderWindow renderWindow)
        {
            RenderWindow = renderWindow;
            RenderWindow.Closed += HandleClose;
            RenderWindow.Resized += HandleResize;

            _context = new();
            _context.Bind(RenderWindow);
            _controller = new();
            _controller.Bind(_context);

            var projection = GetDefaultProjection();
            RenderWindow.PushProjection(projection);
        }

        public void Bind(KeyboardListener keyboardListener)
        {
            _keyboardListener = keyboardListener;
            _keyboardListener.Bind(RenderWindow);
            _controller.Bind(_keyboardListener);
        }

        public void Bind(MouseListener mouseListener)
        {
            _mouseListener = mouseListener;
            _mouseListener.Bind(RenderWindow);
            _context.Bind(mouseListener);
            _controller.Bind(_mouseListener);

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
                    UiRoot.Update(frameElapsed - elapsed);
                    UiRoot.Draw(RenderWindow, _context);
                }
                _keyboardListener?.DispatchEvents(frameElapsed - elapsed);
                Console.WriteLine(frameElapsed - elapsed);
                _controller.DispatchEvents();
                elapsed = frameElapsed;

                RenderWindow.Display();
            }
        }

        private void HandleClose(object? sender, EventArgs e)
        {
            _run = false;
        }

        private void HandleResize(object? sender, ResizeEventArgs e)
        {
            RenderWindow.SetViewPort(new(new(), e.Size));

            var projection = GetDefaultProjection();
            RenderWindow.PopProjectionMatrix();
            RenderWindow.PushProjection(projection);
        }

        private Projection GetDefaultProjection()
        {
            var viewPort = RenderWindow.GetViewPort();
            return new(-10, Matrix4.CreateOrthographicOffCenter(0, viewPort.Size.X, viewPort.Size.Y, 0, -10, 10));
        }
    }
}
