using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Coordinates
{
    public struct Spherical3
    {
        public float Radius { get; set; }
        public float Zenith { get; set; }
        public float Azimuth { get; set; }

        public Spherical3(float radius, float zenith, float azimuth)
        {
            Radius = radius;
            Zenith = zenith;
            Azimuth = azimuth;
        }

        public Vector3 AsCartesian()
        {
            return new(
                (float)(Radius * Math.Sin(Zenith) * Math.Cos(Azimuth)),
                (float)(Radius * Math.Cos(Zenith)),
                (float)(Radius * Math.Sin(Zenith) * Math.Sin(Azimuth)));
        }

        public Cylindrical3 AsCylindrical()
        {
            return new(Radius * (float)Math.Cos(Zenith), Azimuth, Radius * (float)Math.Sin(Zenith));
        }

        public static Spherical3 operator *(float m, Spherical3 c)
        {
            return new Spherical3(m * c.Radius, c.Zenith, c.Azimuth);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}rad, {2}rad)", Radius, Zenith, Azimuth);
        }
    }
}
