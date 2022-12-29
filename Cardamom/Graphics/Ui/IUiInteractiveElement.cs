using Cardamom.Mathematics.Geometry;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
{
    public interface IUiInteractiveElement : IUiElement
    {
        float? GetRayIntersection(Ray3 ray);
    }
}
