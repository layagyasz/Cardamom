using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Geometry
{
    public class Sphere : ICollider3
    {
        public Vector3 Center { get; }
        public float Radius { get; }

        public Sphere(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public float? GetRayIntersection(Ray3 ray)
        {
            var q = ray.Point - Center;
            var (t1, t2) = 
                Quadratic.Solve(
                    ray.Direction.LengthSquared, 
                    2 * Vector3.Dot(ray.Direction, q), 
                    q.LengthSquared - Radius * Radius);
            if (t1 == null || t2 == null)
            {
                return null;
            }
            if (t1 < 0)
            {
                return t2;
            }
            if (t2 < 0)
            {
                return t1;
            }
            return Math.Min(t1.Value, t2.Value);
        }
    }
}
