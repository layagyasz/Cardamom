namespace Cardamom.Randoms
{
    public class ConstantSampler : ISampler
    {
        public float Value { get; set; }

        public ConstantSampler() { }

        public ConstantSampler(float value)
        {
            Value = value;
        }

        public float Generate(Random random)
        {
            return Value;
        }
    }
}
