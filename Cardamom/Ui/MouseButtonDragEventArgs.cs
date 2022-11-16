using SFML.System;
using SFML.Window;

namespace Cardamom.Ui
{
    public class MouseButtonDragEventArgs : EventArgs
    {
        public Mouse.Button Button { get; }
        public Vector2f Start { get; }
        public Vector2f End { get; }
        public Vector2f Delta { get; }

        public MouseButtonDragEventArgs(Mouse.Button button, Vector2f start, Vector2f end, Vector2f delta)
        {
            Button = button;
            Start = start;
            End = end;
            Delta = delta;
        }
    }
}
