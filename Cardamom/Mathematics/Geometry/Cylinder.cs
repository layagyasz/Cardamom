using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Geometry
{
    public class Cylinder : ICollider3
    {
        public Vector3 Center { get; }
        public Vector3 Axis { get; }
        public float Radius { get; }

        public Cylinder(Vector3 center, Vector3 axis, float radius)
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
            return Sphere.GetRayIntersection(
                new(), 
                radius, 
                new(
                    Extensions.Rejection(ray.Point - center, axis), 
                    Extensions.Rejection(ray.Direction, axis)));
        }
    }
}
