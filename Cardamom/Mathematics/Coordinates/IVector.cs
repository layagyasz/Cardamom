namespace Cardamom.Mathematics.Coordinates
{
    public interface IVector
    {
        public int Cardinality { get; }

        public float this[int index] { get; set; }

        public IVector Clone();

        public static float DistanceSquared(IVector left, IVector right)
        {
            Precondition.Check(left.Cardinality == right.Cardinality);
            float d2 = 0;
            for (int i = 0; i < left.Cardinality; ++i)
            {
                float d = left[i] - right[i];
                d2 += d * d;
            }
            return d2;
        }
    }
}
