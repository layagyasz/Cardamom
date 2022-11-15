using Cardamom.Ui.Controller;
using SFML.System;

namespace Cardamom.Ui
{
    public interface IUiElement : IRenderable, IControlled
    {
        bool Visible { get; set; }
        Vector2f Position { get; set; }
        Vector2f Size { get; }
    }
}
