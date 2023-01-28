namespace Cardamom.Utils.Generators.Numeric
{
    public class SeedGenerator : IGenerator<int>
    {
        public int Generate(Random random)
        {
            return random.Next(int.MinValue, int.MaxValue);
        }
    }
}
