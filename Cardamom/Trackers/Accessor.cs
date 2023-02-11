namespace Cardamom.Trackers
{
    public class Accessor<T> where T : struct
    {
        public T Value { get; set; }

        public Accessor(T value)
        {
            Value = value;
        }
    }
}
