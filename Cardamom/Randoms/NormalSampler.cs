using MathNet.Numerics.Distributions;

namespace Cardamom.Randoms
{
    public class NormalSampler : ISampler
    {
        public float Mean { get; set; }
        public float StandardDeviation { get; set; }

        public float Generate(Random random)
        {
            return (float)Normal.Sample(random, Mean, StandardDeviation);
        }
    }
}
