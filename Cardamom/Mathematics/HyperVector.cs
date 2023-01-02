namespace Cardamom.Mathematics
{
    public struct HyperVector
    {
        public int Cardinality => _values.Length;

        private readonly float[] _values;

        public float this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }

        public HyperVector(params float[] values)
        {
            _values = values;
        }

        public HyperVector(int cardinality)
        {
            _values = new float[cardinality];
        }

        public HyperVector Clone()
        {
            return new HyperVector(_values.ToArray());
        }

        public static float DistanceSquared(HyperVector left,  HyperVector right)
        {
            Precondition.Check(left.Cardinality == right.Cardinality);
            float d2 = 0;
            for (int i=0; i<left.Cardinality; ++i)
            {
                float d = left._values[i] - right._values[i];
                d2 += d * d;
            }
            return d2;
        }
    }
}
