using Cardamom.Json.Mathematics;
using OpenTK.Mathematics;
using System.Text.Json.Serialization;

namespace Cardamom.Mathematics
{
    [JsonConverter(typeof(IntervalJsonConverter))]
    public struct Interval
    {
        public static readonly Interval Unbounded = new(float.MinValue, float.MaxValue);

        public float Minimum { get; set; }
        public float Maximum { get; set; }

        public Interval() { }

        public Interval(float minimum, float maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public void Bound(float value)
        {
            Minimum = Math.Min(Minimum, value);
            Maximum = Math.Max(Maximum, value);
        }

        public float Clamp(float value)
        {
            return MathHelper.Clamp(value, Minimum, Maximum);
        }

        public bool Contains(float value)
        {
            return Minimum <= value && value <= Maximum;
        }

        public override string ToString()
        {
            return string.Format($"[{Minimum}, {Maximum}]");
        }
    }
}
