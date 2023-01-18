namespace Cardamom.Utils.Generators.Generic
{
    public interface IGenerator
    {
        T Generate<T>(Random random);
    }
}
