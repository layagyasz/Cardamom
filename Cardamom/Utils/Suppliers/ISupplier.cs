namespace Cardamom.Utils.Suppliers
{
    public interface ISupplier<T> : Generic.ISupplier
    {
        T Get();

        TOut Generic.ISupplier.Get<TOut>()
        {
            if (typeof(TOut).Equals(typeof(T)))
            {
                return (TOut)(object)Get()!;
            }
            throw new ArgumentException($"Cannot implicitly convert type {typeof(T)} to {typeof(TOut)}.");
        }
    }
}
