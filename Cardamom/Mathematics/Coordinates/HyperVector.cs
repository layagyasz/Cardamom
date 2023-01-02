namespace Cardamom.Mathematics.Coordinates
{
    public class HyperVector : IVector
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

        public IVector Clone()
        {
            return new HyperVector(_values.ToArray());
        }
    }
}
