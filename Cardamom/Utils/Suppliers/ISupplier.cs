namespace Cardamom.Utils.Suppliers
{
    public interface ISupplier<T> : Generic.ISupplier
    {
        T Get();

        TOut Generic.ISupplier.Get<TOut>()
        {
            return (TOut)(object)Get()!;
        }
    }
}
