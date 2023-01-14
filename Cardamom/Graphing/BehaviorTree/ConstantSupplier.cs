namespace Cardamom.Graphing.BehaviorTree
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
