namespace Cardamom.Trackers
{
    public class Count<T>
    {
        public T Key { get; set; }
        public int Value { get; set; }

        private Count(T key, int value)
        {
            Key = key;
            Value = value;
        }

        public static Count<T> Create(T key, int value)
        {
            return new Count<T>(key, value);
        }

        public override string ToString()
        {
            return $"[Count: Key={Key}, Value={Value}]";
        }
    }
}
