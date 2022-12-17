using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Coordinates
{
    public class Spherical3
    {
        public float Radius { get; set; }
        public float Theta { get; set; }
        public float Phi { get; set; }

        public Spherical3(float Radius, float Theta, float Phi)
        {
            this.Radius = Radius;
            this.Theta = Theta;
            this.Phi = Phi;
        }

        public Vector3 ToCartesian()
        {
            return new Vector3(
                (float)(Radius * Math.Cos(Phi) * Math.Sin(Theta)),
                (float)(Radius * Math.Sin(Phi) * Math.Sin(Theta)),
                (float)(Radius * Math.Cos(Theta)));
        }

        public static Spherical3 FromCartesian(Vector3 Vector)
        {
            return new Spherical3(
                (float)Math.Sqrt(Vector.X * Vector.X + Vector.Y * Vector.Y + Vector.Z * Vector.Z),
                (float)Math.Atan2(Math.Sqrt(Vector.X * Vector.X + Vector.Y * Vector.Y), Vector.Z),
                (float)Math.Atan2(Vector.Y, Vector.X));
        }

        public static Spherical3 operator *(float M, Spherical3 C)
        {
            return new Spherical3(M * C.Radius, C.Theta, C.Phi);
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]", Radius, Theta, Phi);
        }
    }
}
