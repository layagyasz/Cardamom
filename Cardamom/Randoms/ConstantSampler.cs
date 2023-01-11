namespace Cardamom.Randoms
{
    public class ConstantSampler : ISampler
    {
        public float Value { get; set; }

        public float Generate(Random random)
        {
            return Value;
        }
    }
}
