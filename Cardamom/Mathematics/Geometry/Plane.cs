using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Geometry
{
    public class Plane : ICollider3
    {
        public Vector3 Center { get; }
        public Vector3 Axis { get; }

        public Plane(Vector3 center, Vector3 axis)
        {
            Center = center;
            Axis = axis;
        }

        public float? GetRayIntersection(Ray3 ray)
        {
            return GetRayIntersection(Center, Axis, ray);
        }

        public static float? GetRayIntersection(Vector3 center, Vector3 axis, Ray3 ray)
        {
            float d = Vector3.Dot(axis, ray.Direction);
            if (Math.Abs(d) < float.Epsilon)
            {
                return null;
            }
            return Vector3.Dot(center - ray.Point, axis) / d;
        }
    }
}
