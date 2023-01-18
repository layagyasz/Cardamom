using MathNet.Numerics.Distributions;

namespace Cardamom.Utils.Generators.Samplers
{
    public class NormalSampler : ISampler
    {
        public float Mean { get; set; }
        public float StandardDeviation { get; set; }

        public NormalSampler() { }

        public NormalSampler(float mean, float standardDeviation)
        {
            Mean = mean;
            StandardDeviation = standardDeviation;
        }

        public float Generate(Random random)
        {
            return (float)Normal.Sample(random, Mean, StandardDeviation);
        }
    }
}
