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

        public static Vector3 SurfaceNormal(Vector3[] face)
        {
            Vector3 n = new();
            for (int i = 0; i < 3; ++i)
            {
                var current = face[i];
                var next = face[(i + 1) % 3];
                n.X += (current.Y - next.Y) * (current.Z + next.Z);
                n.Y += (current.Z - next.Z) * (current.X + next.X);
                n.Z += (current.X - next.X) * (current.Y + next.Y);
            }
            return n.Normalized();
        }
    }
}
