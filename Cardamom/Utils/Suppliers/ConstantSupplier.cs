namespace Cardamom.Utils.Suppliers
{
    public class ConstantSupplier<T> : IConstantSupplier<T>
    {
        public T? Value { get; set; }

        public ConstantSupplier() { }

        public ConstantSupplier(T value)
        {
            Value = value;
        }

        public static ConstantSupplier<TOut> Create<TOut>(TOut value)
        {
            return new ConstantSupplier<TOut>(value);
        }

        public T Get()
        {
            return Value!;
        }
    }
}
