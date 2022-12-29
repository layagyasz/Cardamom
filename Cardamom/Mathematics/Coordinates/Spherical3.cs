using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Coordinates
{
    public struct Spherical3
    {
        public float Radius { get; set; }
        public float Theta { get; set; }
        public float Phi { get; set; }

        public Spherical3(float radius, float theta, float phi)
        {
            Radius = radius;
            Theta = theta;
            Phi = phi;
        }

        public Vector3 AsCartesian()
        {
            return new(
                (float)(Radius * Math.Cos(Theta) * Math.Cos(Phi)),
                (float)(Radius * Math.Sin(Theta)),
                (float)(Radius * Math.Cos(Theta) * Math.Sin(Phi)));
        }

        public Cylindrical3 AsCylindrical()
        {
            return new(Radius * (float)Math.Cos(Theta), Phi, Radius * (float)Math.Sin(Theta));
        }

        public static Spherical3 operator *(float m, Spherical3 c)
        {
            return new Spherical3(m * c.Radius, c.Theta, c.Phi);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}rad, {2}rad)", Radius, Theta, Phi);
        }
    }
}
