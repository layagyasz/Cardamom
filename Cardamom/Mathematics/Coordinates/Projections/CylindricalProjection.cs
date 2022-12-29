using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Coordinates.Projections
{
    public static class CylindricalProjection
    {
        private static readonly float s_OneOverPi = (float)(1.0 / Math.PI);

        public class Cartesian : IProjection<Vector3, Vector2>
        {
            private readonly Spherical _internalProjection;

            public Vector2 Project(Vector3 coordinate)
            {
                return _internalProjection.Project(coordinate.AsSpherical());
            }

            public Vector3 Wrap(Vector2 coordinate)
            {
                return _internalProjection.Wrap(coordinate).AsCartesian();
            }
        }

        public class Cylindrical : IProjection<Cylindrical3, Vector2>
        {
            public Vector2 Project(Cylindrical3 coordinate)
            {
                return new(
                    s_OneOverPi * coordinate.Azimuth, 
                    2 * s_OneOverPi * (float)Math.Atan2(coordinate.Y, coordinate.Radius));
            }

            public Cylindrical3 Wrap(Vector2 coordinate)
            {
                return new(1, (float)(coordinate.X * Math.PI), (float)Math.Tan(coordinate.Y * MathHelper.PiOver2));
            }
        }

        public class Spherical : IProjection<Spherical3, Vector2>
        {
            public Vector2 Project(Spherical3 coordinate)
            {
                return new(s_OneOverPi * coordinate.Azimuth, 2 * s_OneOverPi * coordinate.Zenith);
            }

            public Spherical3 Wrap(Vector2 coordinate)
            {
                return new(1, (float)(coordinate.X * Math.PI), coordinate.Y * MathHelper.PiOver2);
            }
        }
    }
}
