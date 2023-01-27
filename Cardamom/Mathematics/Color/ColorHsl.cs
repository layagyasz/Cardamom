using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Color
{
    public struct ColorHsl
    {
        public float H { get; set; }
        public float S { get; set;}
        public float L { get; set; }
        public float A { get; set; }

        public ColorHsl(float h, float s, float l, float a)
        {
            H = h;
            S = s;
            L = l;
            A = a;
        }

        public Color4 AsRgb()
        {
            if (S < float.Epsilon)
            {
                return new Color4(L, L, L, A);
            }
            float q = 
                L < 0.5f ? L * (1 + S) : L + S - L * S;
            float p = 2 * L - q;
            float r = HueToRGB(p, q, H + .3333333f);
            float g = HueToRGB(p, q, H);
            float b = HueToRGB(p, q, H - .3333333f);
            return new(r, g, b, A);
        }

        private static float HueToRGB(float p, float q, float t)
        {
            if (t < 0)
            {
                t += 1;
            }
            if (t > 1)
            {
                t -= 1;
            }
            if (t < .16666666667f)
            {
                return p + (q - p) * 6 * t;
            }
            if (t < .5)
            {
                return q;
            }
            if (t < .666666667f)
            {
                return p + (q - p) * (.6666666667f - t) * 6;
            }
            return p;
        }
    }
}
