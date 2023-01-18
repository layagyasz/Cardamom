namespace Cardamom.Utils.Suppliers
{
    public class ConstantSupplier<T> : ISupplier<T>
    {
        public T Value { get; }

        public ConstantSupplier(T value)
        {
            Value = value;
        }

        public T Get()
        {
            return Value;
        }
    }
}
