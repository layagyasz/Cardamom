namespace Cardamom.Randoms
{
    public class ReciprocalSampler : ISampler
    {
        public float Support { get; set; }
        public float Scale { get; set; }

        public ReciprocalSampler() { }

        public ReciprocalSampler(float support, float scale)
        {
            Support = support;
            Scale = scale;
        }

        public float Generate(Random random)
        {
            return Support * (float)Math.Pow(Scale, random.NextDouble());
        }
    }
}
