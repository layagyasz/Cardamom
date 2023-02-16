using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Geometry
{
    public class Disk : ICollider3
    {
        public Vector3 Center { get; }
        public Vector3 Axis { get; }
        public float Radius { get; }

        public Disk(Vector3 center, Vector3 axis, float radius)
        {
            Center = center;
            Axis = axis;
            Radius = radius;
        }

        public float? GetRayIntersection(Ray3 ray)
        {
            return GetRayIntersection(Center, Axis, Radius, ray);
        }

        public static float? GetRayIntersection(Vector3 center, Vector3 axis, float radius, Ray3 ray)
        {
            var t = Plane.GetRayIntersection(center, axis, ray);
            if (t == null)
            {
                return null;
            }
            var p = ray.Get(t!.Value) - center;
            return Vector3.Dot(p, p) < radius * radius ? t : null;
        }
    }
}
