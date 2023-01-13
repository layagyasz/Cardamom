using MathNet.Numerics.Distributions;

namespace Cardamom.Randoms
{
    public class ExponentialSampler : ISampler
    {
        public float Scale { get; set; }
        public float Rate { get; set; }

        public ExponentialSampler() { }

        public ExponentialSampler(float scale, float rate) 
        {
            Scale = scale;
            Rate = rate;
        }

        public float Generate(Random random)
        {
            return Scale * (float)Exponential.Sample(random, Rate);
        }
    }
}
