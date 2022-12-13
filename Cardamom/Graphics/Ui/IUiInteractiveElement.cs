using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
{
    public interface IUiInteractiveElement : IUiElement
    {
        bool IntersectsRay(Vector3 origin, Vector3 direction);
    }
}
