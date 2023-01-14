namespace Cardamom.Graphing.BehaviorTree
{
    public class Cache<T> : ISupplier<T>
    {
        protected T? _value;
        private bool _valid;

        public Cache()
        {
            _value = GetDefault();
        }

        public bool IsPresent()
        {
            return _valid;
        }

        public void Set(T value)
        {
            _value = value;
            _valid = true;
        }

        public T Get()
        {
            return _value!;
        }

        public void Invalidate()
        {
            _value = default;
            _valid = false;
        }

        protected virtual T? GetDefault()
        {
            return default;
        }
    }
}