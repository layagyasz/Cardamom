namespace Cardamom.Mathematics
{
    public static class Quadratic
    {
        public static Tuple<float?, float?> Solve(float a, float b, float c)
        {
            var det = b * b - 4 * a * c;
            if (det < 0)
            {
                return new(null, null);
            }
            return new((-b + MathF.Sqrt(det)) / (2 * a), (-b - MathF.Sqrt(det)) / (2 * a));
        }
    }
}
