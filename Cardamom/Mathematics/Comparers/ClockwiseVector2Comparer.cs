using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Comparers
{
    class ClockwiseVector2Comparer : IComparer<Vector2>
    {
        public Vector2 Center { get; }

        public ClockwiseVector2Comparer(Vector2 center)
        {
            Center = center;
        }

        public int Compare(Vector2 a, Vector2 b)
        {
            var ac = a - Center;
            var bc = b - Center;

            if (ac.X >= 0 && bc.X < 0)
            {
                return 1;
            }
            if (ac.X < 0 && bc.X >= 0)
            {
                return -1;
            }

            return ac.X * bc.Y - ac.Y * bc.X < 0 ? 1 : -1;
        }
    }
}