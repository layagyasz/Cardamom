using MathNet.Numerics.Distributions;

namespace Cardamom.Utils.Generators.Samplers
{
    public class ParetoSampler : ISampler
    {
        public float Scale { get; set; }
        public float Shape { get; set; }

        public ParetoSampler() { }

        public ParetoSampler(float scale, float shape)
        {
            Scale = scale;
            Shape = shape;
        }

        public float Generate(Random random)
        {
            return (float)Pareto.Sample(random, Scale, Shape);
        }
    }
}
