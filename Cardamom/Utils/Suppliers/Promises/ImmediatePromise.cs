namespace Cardamom.Utils.Suppliers.Promises
{
    public class ImmediatePromise<T> : IPromise<T>
    {
        public EventHandler<EventArgs>? Canceled { get; set; }
        public EventHandler<EventArgs>? Finished { get; set; }

        private readonly T _value;

        private ImmediatePromise(T value)
        {
            _value = value;
        }

        public static ImmediatePromise<T> Of(T value)
        {
            return new(value);
        }

        public T Get()
        {
            return _value;
        }

        public bool HasValue()
        {
            return true;
        }
    }
}
