namespace Cardamom.Utils.Generators
{
    public interface IGenerator<T> : Generic.IGenerator
    {
        T Generate(Random random);

        TOut Generic.IGenerator.Generate<TOut>(Random random)
        {
            return (TOut)(object)Generate(random)!;
        }
    }
}
