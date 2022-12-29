using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Graphics.Ui
{
    public struct MouseButtonClickEventArgs
    {
        public InputAction Action { get; set; }
        public MouseButton Button { get; set; }
        public KeyModifiers Modifiers { get; set; }
        public bool IsPressed { get; set; }
        public Vector3 Position { get; set; }

        public override string ToString()
        {
            return string.Format(
                $"[MouseButtonClickEvent: Action={Action}, Button={Button}, Modifiers={Modifiers}, " 
                + $"IsPressed={IsPressed}, Position={Position}]");
        }
    }
}
