using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Coordinates
{
    public static class Extensions
    {
        public static Cylindrical3 AsCylindrical(this Vector3 v)
        {
            return new((float)Math.Sqrt(v.X * v.X + v.Z * v.Z), (float)Math.Atan2(v.Z, v.X), v.Y);
        }

        public static Polar2 AsPolar(this Vector2 v)
        {
            return new(v.Length, (float)Math.Atan2(v.Y, v.X));
        }

        public static Spherical3 AsSpherical(this Vector3 v)
        {
            return new(
                (float)Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z),
                (float)Math.Atan2(v.Y, Math.Sqrt(v.X * v.X + v.Z * v.Z)),
                (float)Math.Atan2(v.Z, v.X));
        }
    }
}
