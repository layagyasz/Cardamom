using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Window
{
    public readonly struct MouseButtonDragEventArgs
    {
        public MouseButton Button { get; }
        public Vector2 Start { get; }
        public Vector2 End { get; }
        public Vector2 Delta { get; }
        public Vector2 NdcDelta { get; }
        public bool Terminate { get; }

        public MouseButtonDragEventArgs(
            MouseButton button, Vector2 start, Vector2 end, Vector2 delta, Vector2 ndcDelta, bool terminate)
        {
            Button = button;
            Start = start;
            End = end;
            Delta = delta;
            NdcDelta = ndcDelta;
            Terminate = terminate;
        }

        public override string ToString()
        {
            return $"[MouseButtonDragEventArgs: Button={Button}, Start={Start}, End={End}, Delta={Delta}," 
                + " Terminate={Terminate}]";
        }
    }
}
