using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public struct Projection
    {
        public float NearPlane { get; set; }
        public Matrix4 Matrix { get; set; }

        public Projection(float nearPlane, Matrix4 matrix)
        {
            NearPlane = nearPlane;
            Matrix = matrix;
        }
    }
}
