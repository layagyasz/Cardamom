using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Comparers
{
    public class ClockwiseVector3Comparer : IComparer<Vector3>
    {
        public Vector3 Center { get; }
        public Vector3 Normal { get; }

        public ClockwiseVector3Comparer(Vector3 center, Vector3 normal)
        {
            Center = center;
            Normal = normal;
        }

        public int Compare(Vector3 a, Vector3 b)
        {
            return Vector3.Dot(Normal, Vector3.Cross(a - Center, b - Center)) < 0 ? 1 : -1;
        }
    }
}
