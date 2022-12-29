using Cardamom.Mathematics.Geometry;

namespace Cardamom.Graphics.Ui
{
    public interface IUiInteractiveElement : IUiElement
    {
        float? GetRayIntersection(Ray3 ray);
    }
}
