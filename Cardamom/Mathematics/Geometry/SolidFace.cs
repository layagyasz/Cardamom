using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Geometry
{
    public class SolidFace
    {
        public Vector3[] Vertices { get; }

        public SolidFace(params Vector3[] vertices)
        {
            Vertices = vertices;
        }
    }
}
