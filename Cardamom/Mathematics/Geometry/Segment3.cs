using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Geometry
{
    public struct Segment3
    {
        public Vector3 Left { get; set; }
        public Vector3 Right { get; set; }

        public Segment3(Vector3 left, Vector3 right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return string.Format($"[Segment: Left={Left}, Right={Right}]");
        }
    }
}
