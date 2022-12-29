using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Coordinates
{
    public struct Cylindrical3
    {
        public float Radius { get; set; }
        public float Azimuth { get; set; }
        public float Y { get; set; }

        public Cylindrical3(float radius, float azimuth, float y)
        {
            Radius = radius;
            Azimuth = azimuth;
            Y = y;
        }
        
        public Vector3 AsCartesian()
        {
            return new(Radius * (float)Math.Cos(Azimuth), Y, Radius * (float)Math.Sin(Azimuth));
        }

        public Spherical3 AsSpherical()
        {
            return new((float)Math.Sqrt(Radius * Radius + Y * Y), Azimuth, (float)Math.Atan2(Y, Radius));
        }

        public static Cylindrical3 operator *(float m, Cylindrical3 c)
        {
            return new Cylindrical3(m * c.Radius, c.Azimuth, c.Y);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}rad, {2})", Radius, Azimuth, Y);
        }
    }
}
