using MathNet.Numerics.Distributions;

namespace Cardamom.Randoms
{
    public class ExponentialSampler : ISampler
    {
        public float Scale { get; set; }
        public float Rate { get; set; }

        public float Generate(Random random)
        {
            return Scale * (float)Exponential.Sample(random, Rate);
        }
    }
}
