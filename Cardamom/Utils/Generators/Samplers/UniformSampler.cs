using Cardamom.Mathematics;

namespace Cardamom.Utils.Generators.Samplers
{
    public class UniformSampler : ISampler
    {
        public Interval Range { get; set; }

        public UniformSampler() { }

        public UniformSampler(Interval range)
        {
            Range = range;
        }

        public float Generate(Random random)
        {
            return Range.Minimum + random.NextSingle() * (Range.Maximum - Range.Minimum);
        }
    }
}
