using Cardamom.Graphics;
using Cardamom.Ui.Controller.Element;
using Cardamom.Window;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System.Diagnostics;

namespace Cardamom.Ui
{
    public class UiWindow
    {
        public RenderWindow RenderWindow { get; }

        private readonly UiRootController _controller;
        private readonly SimpleUiContext _context;

        private IRenderable? _uiRoot;
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
            RenderWindow.PushViewMatrix(Matrix4.Identity);
            RenderWindow.PushModelMatrix(Matrix4.Identity);
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

        public void SetRoot(IRenderable root)
        {
            _uiRoot = root;
            _uiRoot.Initialize();
            Vector2 viewport = RenderWindow.GetViewPort().Size;
            _uiRoot.ResizeContext(new(viewport.X, viewport.Y, 0));
        }

        public void SetFocus(IControlledElement element)
        {
            _controller.SetFocus(element);
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
                long delta = frameElapsed - elapsed;
                if (_uiRoot != null)
                {
                    _uiRoot.Update(delta);
                    _uiRoot.Draw(RenderWindow, _context);
                }
                _keyboardListener?.DispatchEvents(delta);
                _mouseListener?.DispatchEvents(delta);
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

            _uiRoot?.ResizeContext(new(e.Size.X, e.Size.Y, 0));
        }

        private Projection GetDefaultProjection()
        {
            var viewPort = RenderWindow.GetViewPort();
            return new(-10, Matrix4.CreateOrthographicOffCenter(0, viewPort.Size.X, viewPort.Size.Y, 0, -10, 10));
        }
    }
}
