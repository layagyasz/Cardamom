using Cardamom.Mathematics;

namespace Cardamom.Randoms
{
    public class UniformSampler : ISampler
    {
        public Interval Range { get; set; }

        public float Generate(Random random)
        {
            return Range.Minimum + random.NextSingle() * (Range.Maximum - Range.Minimum);
        }
    }
}
