﻿using OpenTK.Mathematics;

namespace Cardamom.Mathematics
{
    public struct FloatRange
    {
        public static readonly FloatRange Unbounded = new(float.MinValue, float.MaxValue);

        public float Minimum { get; set; }
        public float Maximum { get; set; }

        public FloatRange() { }

        public FloatRange(float minimum, float maximum)
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

        public override string ToString()
        {
            return string.Format($"[{Minimum}, {Maximum}]");
        }
    }
}
