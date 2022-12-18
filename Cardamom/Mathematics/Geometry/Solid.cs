using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Geometry
{
    public class Solid
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

        public SolidFace[] Faces { get; }

        public Solid(SolidFace[] faces)
        {
            Faces = faces;
        }

        public static Solid GenerateCube(float scale)
        {
            SolidFace[] faces = new SolidFace[6];
            for (int i = 0; i < 6; ++i)
            {
                var face = new Vector3[6];
                for (int j = 0; j < 6; ++j)
                {
                    face[j] = scale * s_CubeVertices[s_CubeFaces[i][j]];
                }
                faces[i] = new SolidFace(face);
            }
            return new Solid(faces);
        }

        public static Solid GenerateUvSphere(float scale, int subdivisions)
        {
            Precondition.Check(subdivisions > 0);
            var longitudesSin = new float[2 * subdivisions + 2];
            var longitudesCos = new float[2 * subdivisions + 2];
            for (int i=0; i<longitudesSin.Length; ++i)
            {
                var angle = i * Math.Tau / longitudesSin.Length;
                longitudesSin[i] = (float)Math.Sin(angle);
                longitudesCos[i] = (float)Math.Cos(angle);
            }
            var faces = new SolidFace[(subdivisions + 2) * longitudesSin.Length];
            int k = 0;
            float lastRowSin = 0;
            float lastRowCos = 0;
            for (int i = 0; i <= subdivisions + 1; ++i)
            {
                float angle = (i + 1) * MathHelper.Pi / (subdivisions + 2);
                float rowSin = (float)Math.Sin(angle);
                float rowCos = (float)Math.Cos(angle);
                for (int j = 0; j < longitudesSin.Length; ++j)
                {
                    // Positive pole
                    if (i == 0)
                    {
                        faces[k++] =
                            new SolidFace(
                                new(
                                    scale * longitudesCos[j] * rowSin,
                                    scale * longitudesSin[j] * rowSin,
                                    scale * rowCos),
                                new(
                                    scale * longitudesCos[(j + 1) % longitudesCos.Length] * rowSin,
                                    scale * longitudesSin[(j + 1) % longitudesSin.Length] * rowSin,
                                    scale * rowCos),
                                new Vector3(0, 0, scale));
                    }
                    // Negative pole
                    else if (i == subdivisions + 1)
                    {
                        faces[k++] =
                            new SolidFace(
                                new(
                                    scale * longitudesCos[j] * lastRowSin,
                                    scale * longitudesSin[j] * lastRowSin,
                                    scale * lastRowCos),
                                new(
                                    scale * longitudesCos[(j + 1) % longitudesCos.Length] * lastRowSin,
                                    scale * longitudesSin[(j + 1) % longitudesSin.Length] * lastRowSin,
                                    scale * lastRowCos),
                                new Vector3(0, 0, -scale));
                    }
                    // Body
                    else
                    {
                        var b = 
                            new Vector3(
                                scale * longitudesCos[(j + 1) % longitudesCos.Length] * lastRowSin,
                                scale * longitudesSin[(j + 1) % longitudesSin.Length] * lastRowSin,
                                scale * lastRowCos);
                        var c = new Vector3(
                                scale * longitudesCos[j] * rowSin,
                                scale * longitudesSin[j] * rowSin,
                                scale * rowCos);
                        faces[k++] =
                            new SolidFace(
                                new(
                                    scale * longitudesCos[j] * lastRowSin,
                                    scale * longitudesSin[j] * lastRowSin,
                                    scale * lastRowCos),
                                b,
                                c,
                                c,
                                b,
                                new(
                                    scale * longitudesCos[(j + 1) % longitudesCos.Length] * rowSin,
                                    scale * longitudesSin[(j + 1) % longitudesSin.Length] * rowSin,
                                    scale * rowCos));
                    }
                }
                lastRowCos = rowCos;
                lastRowSin = rowSin;
            }
            return new Solid(faces);
        }

        public static Solid GenerateIcosphere(float scale, int subdivisions)
        {
            Precondition.Check(subdivisions > 0);
            SolidFace[] faces = new SolidFace[20 * subdivisions * subdivisions];
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
                        Subdivide(new SolidFace(face), subdivisions, scale),
                        0,
                        faces,
                        i * subdivisions * subdivisions,
                        subdivisions * subdivisions);
                }
                else
                {
                    faces[i] = new SolidFace(face);
                }
            }
            return new Solid(faces);
        }

        private static SolidFace[] Subdivide(SolidFace face, int subdivisions, float scale)
        {
            var faces = new SolidFace[subdivisions * subdivisions];
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
                            faces[k++] =
                                new SolidFace(points[i][j - 1], points[i - 1][j - 1], points[i - 1][j - 2]);
                        }
                        faces[k++] = new SolidFace(points[i - 1][j - 1], points[i][j - 1], points[i][j]);
                    }
                }
            }
            return faces;
        }
    }
}
