using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Geometry
{
    public struct  Ray3
    {
        public Vector3 Point { get; set; }
        public Vector3 Direction { get; set; }

        public Ray3(Vector3 point,  Vector3 direction)
        {
            Point = point;
            Direction = direction;
        }
    }
}
