using Cardamom.Mathematics.Coordinates;
using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Geometry
{
    public class Solid<TSystem>
    {
        private static readonly Vector3[] s_CubeVertices =
        {
            new(-1, -1, -1),
            new(1, -1, -1),
            new(-1, 1, -1),
            new(1, 1, -1),
            new(-1, -1, 1),
            new(1, -1, 1),
            new(-1, 1, 1),
            new(1, 1, 1),
        };

        private static readonly int[][] s_CubeFaces =
        {
            new int[] { 0, 1, 2, 1, 2, 3 },
            new int[] { 4, 5, 6, 5, 6, 7 },
            new int[] { 0, 2, 4, 2, 4, 6 },
            new int[] { 2, 3, 6, 3, 6, 7 },
            new int[] { 1, 3, 5, 3 ,5, 7 },
            new int[] { 0, 1, 4, 1, 4, 5 }
        };

        private static readonly Vector3[] s_IcosphereVertices =
        {
            new(1,Constants.Phi,0),
            new(-1,Constants.Phi,0),
            new(1,-Constants.Phi,0),
            new(-1,-Constants.Phi,0),
            new(0,1,Constants.Phi),
            new(0,-1,Constants.Phi),
            new(0,1,-Constants.Phi),
            new(0,-1,-Constants.Phi),
            new(Constants.Phi,0,1),
            new(-Constants.Phi,0,1),
            new(Constants.Phi,0,-1),
            new(-Constants.Phi,0,-1)
        };

        private static readonly int[][] s_IcosphereFaces = 
        {
            new int[] { 0, 1, 4 },
            new int[] { 1, 9, 4 },
            new int[] { 4, 9, 5 },
            new int[] { 5, 9, 3 },
            new int[] { 2, 3, 7 },
            new int[] { 3, 2, 5 },
            new int[] { 7, 10, 2 },
            new int[] { 0, 8, 10 },
            new int[] { 0, 4, 8 },
            new int[] { 8, 2, 10 },
            new int[] { 8, 4, 5 },
            new int[] { 8, 5, 2 },
            new int[] { 1, 0, 6 },
            new int[] { 11, 1, 6 },
            new int[] { 3, 9, 11 },
            new int[] { 6, 10, 7 },
            new int[] { 3, 11, 7 },
            new int[] { 11, 6, 7 },
            new int[] { 6, 0, 10 },
            new int[] { 9, 1, 11 } 
        };

        public SolidFace<TSystem>[] Faces { get; }

        public Solid(SolidFace<TSystem>[] faces)
        {
            Faces = faces;
        }

        public static Solid<Vector3> GenerateCube(float scale)
        {
            var faces = new SolidFace<Vector3>[6];
            for (int i = 0; i < 6; ++i)
            {
                var face = new Vector3[6];
                for (int j = 0; j < 6; ++j)
                {
                    face[j] = scale * s_CubeVertices[s_CubeFaces[i][j]];
                }
                faces[i] = new(face);
            }
            return new Solid<Vector3>(faces);
        }

        public static Solid<Vector3> GenerateCartesianUvSphere(float scale, int subdivisions)
        {
            Precondition.Check(subdivisions > 0);
            var azimuthSin = new float[2 * subdivisions + 2];
            var azimuthCos = new float[2 * subdivisions + 2];
            for (int i=0; i<azimuthSin.Length; ++i)
            {
                var angle = i * Math.Tau / azimuthSin.Length;
                azimuthSin[i] = (float)Math.Sin(angle);
                azimuthCos[i] = (float)Math.Cos(angle);
            }
            var faces = new SolidFace<Vector3>[(subdivisions + 2) * azimuthSin.Length];
            int k = 0;
            float lastZenithSin = 0;
            float lastZenithCos = 0;
            for (int i = 0; i <= subdivisions + 1; ++i)
            {
                float zenith = (i + 1) * MathHelper.Pi / (subdivisions + 2);
                float zenithSin = (float)Math.Sin(zenith);
                float zenithCos = (float)Math.Cos(zenith);
                for (int j = 0; j < azimuthSin.Length; ++j)
                {
                    // Positive pole
                    if (i == 0)
                    {
                        faces[k++] =
                            new(
                                new(
                                    scale * azimuthCos[j] * zenithSin,
                                    scale * zenithCos,
                                    scale * azimuthSin[j] * zenithSin),
                                new(
                                    scale * azimuthCos[(j + 1) % azimuthCos.Length] * zenithSin,
                                    scale * zenithCos,
                                    scale * azimuthSin[(j + 1) % azimuthSin.Length] * zenithSin),
                                new(0, 0, scale));
                    }
                    // Negative pole
                    else if (i == subdivisions + 1)
                    {
                        faces[k++] =
                            new(
                                new(
                                    scale * azimuthCos[j] * lastZenithSin,
                                    scale * lastZenithCos,
                                    scale * azimuthSin[j] * lastZenithSin),
                                new(
                                    scale * azimuthCos[(j + 1) % azimuthCos.Length] * lastZenithSin,
                                    scale * lastZenithCos,
                                    scale * azimuthSin[(j + 1) % azimuthSin.Length] * lastZenithSin),
                                new(0, 0, -scale));
                    }
                    // Body
                    else
                    {
                        var b = 
                            new Vector3(
                                scale * azimuthCos[(j + 1) % azimuthCos.Length] * lastZenithSin,
                                scale * lastZenithCos,
                                scale * azimuthSin[(j + 1) % azimuthSin.Length] * lastZenithSin);
                        var c = new Vector3(
                                scale * azimuthCos[j] * zenithSin,
                                scale * zenithCos,
                                scale * azimuthSin[j] * zenithSin);
                        faces[k++] =
                            new(
                                new(
                                    scale * azimuthCos[j] * lastZenithSin,
                                    scale * lastZenithCos,
                                    scale * azimuthSin[j] * lastZenithSin),
                                b,
                                c,
                                c,
                                b,
                                new(
                                    scale * azimuthCos[(j + 1) % azimuthCos.Length] * zenithSin,
                                    scale * zenithCos,
                                    scale * azimuthSin[(j + 1) % azimuthSin.Length] * zenithSin));
                    }
                }
                lastZenithCos = zenithCos;
                lastZenithSin = zenithSin;
            }
            return new(faces);
        }

        public static Solid<Spherical3> GenerateSphericalUvSphere(float scale, int subdivisions)
        {
            Precondition.Check(subdivisions > 0);
            var faces = new SolidFace<Spherical3>[(subdivisions + 2) * (2 * subdivisions + 2)];
            int k = 0;
            float lastZenith = 0;
            float azimuthStep = MathHelper.TwoPi / (2 * subdivisions + 2);
            for (int i = 0; i <= subdivisions + 1; ++i)
            {
                float zenith = (i + 1) * MathHelper.Pi / (subdivisions + 2);
                for (int j = 0; j < 2 * subdivisions + 2; ++j)
                {
                    // Positive pole
                    if (i == 0)
                    {
                        faces[k++] =
                            new(
                                new(scale, zenith, azimuthStep * j), 
                                new(scale, zenith, azimuthStep * (j + 1)), 
                                new(scale, lastZenith, azimuthStep * j));
                    }
                    // Negative pole
                    else if (i == subdivisions + 1)
                    {
                        faces[k++] =
                            new(
                                new(scale, lastZenith, azimuthStep * j),
                                new(scale, lastZenith, azimuthStep * (j + 1)),
                                new(scale, zenith, azimuthStep * j));
                    }
                    // Body
                    else
                    {
                        var b = new Spherical3(scale, lastZenith, azimuthStep * (j + 1));
                        var c = new Spherical3(scale, zenith, azimuthStep * j);
                        faces[k++] =
                            new(
                                new(scale, lastZenith, azimuthStep * j),
                                b,
                                c,
                                c,
                                b,
                                new(scale, zenith, azimuthStep * (j + 1)));
                    }
                }
                lastZenith = zenith;
            }
            return new(faces);
        }

        public static Solid<Vector3> GenerateIcosphere(float scale, int subdivisions)
        {
            Precondition.Check(subdivisions > 0);
            var faces = new SolidFace<Vector3>[20 * subdivisions * subdivisions];
            for (int i = 0; i < 20; ++i)
            {
                var face = new Vector3[3];
                for (int j = 0; j < 3; ++j)
                {
                    face[j] = scale * s_IcosphereVertices[s_IcosphereFaces[i][j]].Normalized();
                }
                if (subdivisions > 1)
                {
                    Array.Copy(
                        Subdivide(new(face), subdivisions, scale),
                        0,
                        faces,
                        i * subdivisions * subdivisions,
                        subdivisions * subdivisions);
                }
                else
                {
                    faces[i] = new(face);
                }
            }
            return new(faces);
        }

        private static SolidFace<Vector3>[] Subdivide(SolidFace<Vector3> face, int subdivisions, float scale)
        {
            var faces = new SolidFace<Vector3>[subdivisions * subdivisions];
            var points = new Vector3[subdivisions + 1][];
            int k = 0;
            for (int i=0; i<subdivisions + 1; ++i)
            {
                points[i] = new Vector3[i + 1];
                var left = face.Vertices[0] + (1f * i / subdivisions) * (face.Vertices[1] - face.Vertices[0]);
                var right = face.Vertices[0] + (1f * i / subdivisions) * (face.Vertices[2] - face.Vertices[0]);
                for (int j=0; j<=i; ++j)
                {
                    if (j == 0)
                    {
                        points[i][j] = left;
                    }
                    else
                    {
                        points[i][j] = left + (1f * j / i) * (right - left);
                    }
                    points[i][j] = scale * points[i][j].Normalized();
                    if (i > 0 && j > 0)
                    {
                        if (j > 1)
                        {
                            faces[k++] = new(points[i][j - 1], points[i - 1][j - 1], points[i - 1][j - 2]);
                        }
                        faces[k++] = new(points[i - 1][j - 1], points[i][j - 1], points[i][j]);
                    }
                }
            }
            return faces;
        }
    }
}
