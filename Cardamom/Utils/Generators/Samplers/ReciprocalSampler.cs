namespace Cardamom.Utils.Generators.Samplers
{
    public class ReciprocalSampler : ISampler
    {
        public float Scale { get; set; }
        public float Rate { get; set; }
        public float Support { get; set; }

        public ReciprocalSampler() { }

        public ReciprocalSampler(float scale, float rate, float support)
        {
            Scale = scale;
            Rate = rate;
            Support= support;
        }

        public float Generate(Random random)
        {
            return Scale * (float)(Math.Pow(Rate, random.NextDouble()) - 1f) + Support;
        }
    }
}
