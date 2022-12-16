using OpenTK.Mathematics;

namespace Cardamom.Mathematics
{
    public struct FloatRange
    {
        public static readonly FloatRange UNBOUNDED = new(float.MinValue, float.MaxValue);

        public float Minimum { get; set; }
        public float Maximum { get; set; }

        public FloatRange() { }

        public FloatRange(float minimum, float maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public float Clamp(float value)
        {
            return MathHelper.Clamp(value, Minimum, Maximum);
        }
    }
}
