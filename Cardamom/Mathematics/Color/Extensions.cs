using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Color
{
    public static class Extensions
    {
        public static ColorHsl ToHsl(this Color4 color)
        {
            float max = Math.Max(color.R, Math.Max(color.G, color.B));
            float min = Math.Min(color.R, Math.Min(color.G, color.B));
            float l = 0.5f * (max + min);
            if (max - min < float.Epsilon)
            {
                return new ColorHsl(0, 0, l, color.A);
            }
            float d = max - min;
            float s = (l > 0.5f) ? d / (2 - max - min) : d / (max + min);
            float h;
            if (Math.Abs(color.R - max) < float.Epsilon)
            {
                h = (color.G - color.B) / d + ((color.G < color.B) ? 6 : 0);
            }
            else if (Math.Abs(color.G - max) < float.Epsilon)
            {
                h = 2 + (color.B - color.R) / d;
            }
            else
            {
                h = 4 + (color.R - color.G) / d;
            }
            return new ColorHsl(0.166666667f * h, s, l, color.A);
        }
    }
}
