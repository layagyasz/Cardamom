using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Ui
{
    public record struct MouseButtonClickEventArgs(
        InputAction Action,
        MouseButton Button,
        KeyModifiers Modifierd,
        bool IsPressed, 
        Vector3 Position, 
        Vector2 ScreenPosition) { }
}
