using SFML.System;

namespace Cardamom.Ui
{
    public class MouseButtonDragEventArgs : EventArgs
    {
        public Vector2i Start { get; }
        public Vector2i End { get; }
        public Vector2i Delta { get; }

        public MouseButtonDragEventArgs(Vector2i start, Vector2i end, Vector2i delta)
        {
            Start = start;
            End = end;
            Delta = delta;
        }
    }
}
