using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Geometry
{
    public static class Spline
    {
        public static IEnumerable<Vector3> GetPoint(Line3 line, int divisions, float tension = 0)
        {
            float d = 1f / (divisions + 1);
            for (int i = 0; i < line.Count - 1; ++i)
            {
                var control1 = line[i];
                var control2 = line[i + 1];

                var control0 = i > 0 ? line[i - 1] : line[i];
                var control3 = i < line.Count - 2 ? line[i + 2] : control2;

                for (int j = 0; j <= divisions + 1; ++j)
                {
                    yield return GetPoint(
                        j * d, 
                        control1, 
                        control2,
                        0.5f * (control2 - control0), 
                        0.5f * (control3 - control1),
                        tension);
                }
            }
        }

        public static IEnumerable<Vector3> GetPoints(Line3 line, Vector3[] tangents, int divisions, float tension = 0)
        {
            float d = 1f / (divisions + 1);
            for (int i=0; i<line.Count-1; ++i)
            {
                for (int j =0; j<=divisions+1; ++j)
                {
                    yield return GetPoint(j * d, line[i], line[i + 1], tangents[i], tangents[i + 1], tension);
                }
            }
        }

        public static Vector3 GetPoint(
            float x, Vector3 left, Vector3 right, Vector3 tangentLeft, Vector3 tangentRight, float tension = 0)
        {
            return (1 + x * x * (-3 + x * 2)) * left +
                (1 - tension) * (x * (1 + x * (-2 + x))) * tangentLeft +
                x * x * (3 - 2 * x) * right +
                (1 - tension) * (x * x * (x - 1)) * tangentRight;
        }
    }
}
