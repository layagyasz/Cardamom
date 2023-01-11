using MathNet.Numerics.Distributions;

namespace Cardamom.Randoms
{
    public class GammaSampler : ISampler
    {
        public float Shape { get; set; }
        public float Rate { get; set; }

        public float Generate(Random random)
        {
            return (float)Gamma.Sample(random, Shape, Rate);
        }
    }
}
