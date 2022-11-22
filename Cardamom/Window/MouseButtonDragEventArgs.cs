using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Window
{
    public class MouseButtonDragEventArgs : EventArgs
    {
        public MouseButton Button { get; }
        public Vector2 Start { get; }
        public Vector2 End { get; }
        public Vector2 Delta { get; }

        public MouseButtonDragEventArgs(MouseButton button, Vector2 start, Vector2 end, Vector2 delta)
        {
            Button = button;
            Start = start;
            End = end;
            Delta = delta;
        }
    }
}
