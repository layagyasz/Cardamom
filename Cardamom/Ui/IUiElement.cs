using Cardamom.Graphics;
using Cardamom.Ui.Controller.Element;
using OpenTK.Mathematics;

namespace Cardamom.Ui
{
    public interface IUiElement : IRenderable, IControlledElement, IDisposable
    {
        bool Visible { get; set; }
        Vector3 Position { get; set; }
        Vector3 Size { get; }
    }
}
