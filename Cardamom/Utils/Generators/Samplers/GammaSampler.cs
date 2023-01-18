using MathNet.Numerics.Distributions;

namespace Cardamom.Utils.Generators.Samplers
{
    public class GammaSampler : ISampler
    {
        public float Shape { get; set; }
        public float Rate { get; set; }

        public GammaSampler() { }

        public GammaSampler(float shape, float rate)
        {
            Shape = shape;
            Rate = rate;
        }

        public float Generate(Random random)
        {
            return (float)Gamma.Sample(random, Shape, Rate);
        }
    }
}
