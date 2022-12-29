using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Geometry
{
    public class SolidFace<TSystem>
    {
        public TSystem[] Vertices { get; }

        public SolidFace(params TSystem[] vertices)
        {
            Vertices = vertices;
        }
    }
}
