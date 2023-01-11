namespace Cardamom.Randoms
{
    public interface IGenerator<T>
    {
        T Generate(Random random);
    }
}
