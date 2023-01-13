namespace Cardamom.Trackers
{
    public class Frequent<T>
    {
        public T? Value { get; set; }
        public float Frequency { get; set; }

        public Frequent() { }

        public Frequent(T? value, float frequency)
        {
            Value = value;
            Frequency = frequency;
        }
    }
}
