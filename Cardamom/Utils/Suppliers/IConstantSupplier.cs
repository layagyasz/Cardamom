namespace Cardamom.Utils.Suppliers
{
    public interface IConstantSupplier<T> : ISupplier<T>, Generic.IConstantSupplier
    {
        public T? Value { get; set; }

        void Generic.IConstantSupplier.Set(object value)
        {
            Value = (T)value;
        }
    }
}
