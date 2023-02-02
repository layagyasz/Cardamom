using Cardamom.Mathematics;

namespace Cardamom.Utils.Generators.Numeric
{
    public class FloatGenerator : IGenerator<float>
    {
        public Interval Range { get; set; } = new(0, 1);

        public float Generate(Random random)
        {
            return (Range.Maximum - Range.Minimum) * random.NextSingle() + Range.Minimum;
        }
    }
}
