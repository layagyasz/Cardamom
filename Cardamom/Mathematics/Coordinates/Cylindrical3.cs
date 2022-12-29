using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Coordinates
{
    public struct Cylindrical3
    {
        public float Radius { get; set; }
        public float Phi { get; set; }
        public float Y { get; set; }

        public Cylindrical3(float radius, float phi, float y)
        {
            Radius = radius;
            Phi = phi;
            Y = y;
        }
        
        public Vector3 AsCartesian()
        {
            return new(Radius * (float)Math.Cos(Phi), Y, Radius * (float)Math.Sin(Phi));
        }

        public Spherical3 AsSpherical()
        {
            return new((float)Math.Sqrt(Radius * Radius + Y * Y), Phi, (float)Math.Atan2(Y, Radius));
        }

        public static Cylindrical3 operator *(float m, Cylindrical3 c)
        {
            return new Cylindrical3(m * c.Radius, c.Phi, c.Y);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}rad, {2})", Radius, Phi, Y);
        }
    }
}
