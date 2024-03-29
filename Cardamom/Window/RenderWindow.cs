﻿using Cardamom.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.ComponentModel;

namespace Cardamom.Window
{
    public class RenderWindow : BaseRenderTarget
    {
        public EventHandler<EventArgs>? Closed { get; set; }
        public EventHandler<MouseButtonEventArgs>? MouseButtonPressed { get; set; }
        public EventHandler<MouseButtonEventArgs>? MouseButtonReleased { get; set; }
        public EventHandler<MouseWheelEventArgs>? MouseWheelScrolled { get; set; }
        public EventHandler<MouseMoveEventArgs>? MouseMoved { get; set; }
        public EventHandler<KeyboardKeyEventArgs>? KeyReleased { get; set; }
        public EventHandler<KeyboardKeyEventArgs>? KeyPressed { get; set; }
        public EventHandler<ResizeEventArgs>? Resized { get; set; }

        private readonly NativeWindow _window;

        public RenderWindow(string title, Vector2i size)
            : base(new(new(), size))
        {
            _window = new NativeWindow(
                new NativeWindowSettings()
                {
                    Title = title,
                    Size = size,
                    Flags = ContextFlags.ForwardCompatible
                });
            _window.Context.SwapInterval = 0;
            _window.Closing += HandleClosed;
            _window.MouseDown += HandleMouseDown;
            _window.MouseUp += HandleMouseUp;
            _window.MouseWheel += HandleMouseWheel;
            _window.MouseMove += HandleMouseMove;
            _window.KeyDown += HandleKeyDown;
            _window.KeyUp += HandleKeyUp;
            _window.Resize += HandleResize;
        }

        public static void DispatchEvents()
        {
            NativeWindow.ProcessWindowEvents(/* waitForEvents= */ false);
        }

        public override void SetActive(bool active)
        {
            if (active)
            {
                var viewPort = GetViewPort();
                GL.Viewport(viewPort.Min.X, viewPort.Min.Y, viewPort.Size.X, viewPort.Size.Y);
            }
        }

        public override void Display()
        {
            _window.Context.SwapBuffers();
        }

        public IGLFWGraphicsContext GetContext()
        {
            return _window.Context;
        }

        public Vector2 GetMousePosition()
        {
            return _window.MousePosition;
        }

        public bool IsAnyKeyDown()
        {
            return _window.IsAnyKeyDown;
        }

        public bool IsKeyDown(Keys key)
        {
            return _window.IsKeyDown(key);
        }

        public void MakeCurrent()
        {
            _window.MakeCurrent();
        }

        private void HandleClosed(CancelEventArgs e)
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void HandleMouseDown(MouseButtonEventArgs e)
        {
            MouseButtonPressed?.Invoke(this, e);
        }

        private void HandleMouseUp(MouseButtonEventArgs e)
        {
            MouseButtonReleased?.Invoke(this, e);
        }

        private void HandleMouseWheel(MouseWheelEventArgs e)
        {
            MouseWheelScrolled?.Invoke(this, e);
        }

        private void HandleMouseMove(MouseMoveEventArgs e)
        {
            MouseMoved?.Invoke(this, e);
        }

        private void HandleKeyUp(KeyboardKeyEventArgs e)
        {
            KeyReleased?.Invoke(this, e);
        }

        private void HandleKeyDown(KeyboardKeyEventArgs e)
        {
            KeyPressed?.Invoke(this, e);
        }

        private void HandleResize(ResizeEventArgs e)
        {
            Resized?.Invoke(this, e);
        }
    }
}
