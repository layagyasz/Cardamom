using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Coordinates
{
    public struct Polar2
    {
        public float Radius { get; set; }
        public float Azimuth { get; set; }

        public Polar2(float radius, float azimuth)
        {
            Radius = radius;
            Azimuth = azimuth;
        }

        public Vector2 AsCartesian()
        {
            return new(Radius * (float)Math.Cos(Azimuth), Radius * (float)Math.Sin(Azimuth));
        }

        public override string ToString()
        {
            return string.Format($"({Radius}, {Azimuth}rad)");
        }
    }
}
