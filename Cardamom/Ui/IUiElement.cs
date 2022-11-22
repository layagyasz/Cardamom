using Cardamom.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Ui
{
    public interface IUiElement : IRenderable, IControlled
    {
        bool Visible { get; set; }
        Vector2 Position { get; set; }
        Vector2 Size { get; }
    }
}
