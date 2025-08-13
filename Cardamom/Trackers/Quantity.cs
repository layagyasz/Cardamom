namespace Cardamom.Trackers
{
    public record class Quantity<T>
    {
        public T Key { get; set; }
        public float Value { get; set; }

        private Quantity(T key, float value)
        {
            Key = key;
            Value = value;
        }

        public static Quantity<T> Create(T key, float value)
        {
            return new Quantity<T>(key, value);
        }
    }
}
