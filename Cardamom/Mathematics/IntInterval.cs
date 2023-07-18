using Cardamom.Json.Mathematics;
using OpenTK.Mathematics;
using System.Text.Json.Serialization;

namespace Cardamom.Mathematics
{
    [JsonConverter(typeof(IntIntervalJsonConverter))]
    public struct IntInterval
    {
        public static readonly IntInterval Unbounded = new(int.MinValue, int.MaxValue);

        public int Minimum { get; set; }
        public int Maximum { get; set; }

        public IntInterval() { }

        public IntInterval(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public void Bound(int value)
        {
            Minimum = Math.Min(Minimum, value);
            Maximum = Math.Max(Maximum, value);
        }

        public int Clamp(int value)
        {
            return MathHelper.Clamp(value, Minimum, Maximum);
        }

        public bool Contains(int value)
        {
            return Minimum <= value && value <= Maximum;
        }

        public override string ToString()
        {
            return string.Format($"[{Minimum}, {Maximum}]");
        }
    }
}
