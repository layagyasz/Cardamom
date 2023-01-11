using MathNet.Numerics.Distributions;

namespace Cardamom.Randoms
{
    public class ParetoSampler : ISampler
    {
        public float Scale { get; set; }
        public float Shape { get; set; }

        public float Generate(Random random)
        {
            return (float)Pareto.Sample(random, Scale, Shape);
        }
    }
}
