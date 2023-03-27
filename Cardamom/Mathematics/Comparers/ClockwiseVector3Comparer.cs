using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Comparers
{
    public class ClockwiseVector3Comparer : IComparer<Vector3>
    {
        public Vector3 Center { get; }
        public Vector3 Normal { get; }
        public Vector3 Reference { get; }

        public ClockwiseVector3Comparer(Vector3 center, Vector3 normal, Vector3 reference)
        {
            Center = center;
            Normal = normal;
            Reference = reference;
        }

        public int Compare(Vector3 a, Vector3 b)
        {
            var ua = a - Center;
            var ub = b - Center;
            var ha = Vector3.Dot(ua, Reference);
            var hb = Vector3.Dot(ub, Reference);

            if (hb <= 0 && ha > 0)
            {
                return 1;
            }
            if (ha <= 0 && hb > 0)
            {
                return -1;
            }
            if (ha == 0 && hb == 0)
            {
                return Vector3.Dot(ua, Reference) > 0 && Vector3.Dot(ub, Reference) < 0 ? -1 : 1;
            }
            return Vector3.Dot(Normal, Vector3.Cross(ua, ub)) > 0 ? -1 : 1;
        }
    }
}
