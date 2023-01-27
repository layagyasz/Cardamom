using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Color
{
    public class ColorSystem
    {
        public static readonly ColorSystem Ntsc = 
            Create(new(.67f, .33f), new(.21f, .71f), new(.14f, .08f), new (.3101f, .3162f));

        public static readonly ColorSystem Ebu =
            Create(new(.64f, .33f), new(.29f, .6f), new(.15f, .06f), new(.3127f, .3291f));

        public static readonly ColorSystem Smpte =
            Create(new(.63f, .34f), new(.31f, .595f), new(.155f, .07f), new(.3127f, .3291f));

        private readonly Matrix3 _transform;

        private ColorSystem(Matrix3 transform)
        {
            _transform = transform;
        }

        public static ColorSystem Create(ColorCie red, ColorCie green, ColorCie blue, ColorCie white)
        {
            var transformWhite = 
                new Vector3(
                    Vector3.Dot(ToVector(red), ToVector(white)),
                    Vector3.Dot(ToVector(green), ToVector(white)), 
                    Vector3.Dot(ToVector(blue), ToVector(white))) / white.Y;
            var matrix = 
                new Matrix3(
                    new Vector3(red.X, green.X, blue.X) / transformWhite.X,
                    new Vector3(red.Y, green.Y, blue.Y) / transformWhite.Y,
                    new Vector3(red.Z, green.Z, blue.Z) / transformWhite.Z);
            matrix.Transpose();
            return new ColorSystem(matrix);
        }

        public Color4 Transform(ColorCie color)
        {
            var v = ToVector(color);
            var outColor = 
                new Vector3(
                    Vector3.Dot(v, _transform.Row0), Vector3.Dot(v, _transform.Row1), Vector3.Dot(v, _transform.Row2));
            float w = Math.Min(v.X, Math.Min(v.Y, v.Z));
            if (w < 0)
            {
                outColor -= new Vector3(w, w, w);
            }
            float g = Math.Max(v.X, Math.Max(v.Y, v.Z));
            if (g > 0)
            {
                outColor /= g;
            }
            return new Color4(outColor.X, outColor.Y, outColor.Z, 1);
        }

        private static Vector3 ToVector(ColorCie color)
        {
            return new(color.X, color.Y, color.Z);
        }
    }
}
