using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public struct FloatCube
    {
        public Vector3 FrontTopLeft { get; set; }
        public Vector3 Size { get; set; }

        public FloatCube(Vector3 frontTop, Vector3 size)
        {
            FrontTopLeft = frontTop;
            Size = size;
        }
    }
}
