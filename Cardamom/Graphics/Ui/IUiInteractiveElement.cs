using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
{
    public interface IUiInteractiveElement : IUiElement
    {
        bool IsPointWithinBounds(Vector2 point);
    }
}
