namespace Cardamom.Utils.Generators
{
    public interface IGenerator<T> : Generic.IGenerator
    {
        T Generate(Random random);

        TOut Generic.IGenerator.Generate<TOut>(Random random)
        {
            if (typeof(TOut).Equals(typeof(T)))
            {
                return (TOut)(object)Generate(random)!;
            }
            throw new ArgumentException($"Cannot implicitly convert type {typeof(T)} to {typeof(TOut)}.");
        }
    }
}
