using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Coordinates.Projections
{
    public static class StereographicProjection
    {
        public class Cartesian : IProjection<Vector3, Vector2>
        {
            public Vector2 Project(Vector3 coordinate)
            {
                return new(coordinate.X / (1 - coordinate.Y), coordinate.Z / (1 - coordinate.Y));
            }

            public Vector3 Wrap(Vector2 coordinate)
            {
                float denominator = 1 / (1 + coordinate.X * coordinate.X + coordinate.Y * coordinate.Y);
                return new Vector3(
                    2 * denominator * coordinate.X,
                    denominator * (coordinate.X * coordinate.X + coordinate.Y * coordinate.Y - 1),
                    2 * denominator * coordinate.Y);
            }
        }

        public class Cylindrical : IProjection<Cylindrical3, Polar2>
        {
            public Polar2 Project(Cylindrical3 coordinate)
            {
                return new(coordinate.Radius / (1 - coordinate.Y), coordinate.Azimuth);
            }

            public Cylindrical3 Wrap(Polar2 coordinate)
            {
                var r2 = coordinate.Radius * coordinate.Radius;
                return new(2 * coordinate.Radius / (1 + r2), coordinate.Azimuth, (r2 - 1) / (r2 + 1));
            }
        }

        public class Spherical : IProjection<Spherical3, Polar2>
        {
            public Polar2 Project(Spherical3 coordinate)
            {
                return new(1f / (float)Math.Tan(0.5f * coordinate.Zenith), coordinate.Azimuth);
            }

            public Spherical3 Wrap(Polar2 coordinate)
            {
                return new(1, 2 * (float)Math.Atan2(1, coordinate.Radius), coordinate.Azimuth);
            }
        }
    }
}
