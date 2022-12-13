using Cardamom.Graphics.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
{
    public interface IUiElement : IRenderable, IControlled
    {
        bool Visible { get; set; }
        Vector3 Position { get; set; }
        Vector3 Size { get; }
    }
}
