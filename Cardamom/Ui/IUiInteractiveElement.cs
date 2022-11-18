using SFML.System;

namespace Cardamom.Ui
{
    public interface IUiInteractiveElement : IUiElement
    {
        bool IsPointWithinBounds(Vector2f point);
    }
}
