using OpenTK.Mathematics;

namespace Cardamom.Ui
{
    public interface IUiInteractiveElement : IUiElement
    {
        bool IsPointWithinBounds(Vector2 point);
    }
}
