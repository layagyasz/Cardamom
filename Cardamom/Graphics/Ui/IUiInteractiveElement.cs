using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
{
    public interface IUiInteractiveElement : IUiElement
    {
        float? GetRayIntersection(Vector3 origin, Vector3 direction);
    }
}
