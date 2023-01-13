namespace Cardamom.Randoms.Generic
{
    public interface IGenerator
    {
        T Generate<T>(Random random);
    }
}
