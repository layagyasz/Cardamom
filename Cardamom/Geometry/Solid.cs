using OpenTK.Mathematics;

namespace Cardamom.Geometry
{
    public class Solid
    {
        private static readonly Vector3[] CUBE_VERTICES =
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
        private static readonly int[][] CUBE_FACES =
        {
            new int[] { 0, 1, 2, 1, 2, 3 },
            new int[] { 4, 5, 6, 5, 6, 7 },
            new int[] { 0, 2, 4, 2, 4, 6 },
            new int[] { 2, 3, 6, 3, 6, 7 },
            new int[] { 1, 3, 5, 3 ,5, 7 },
            new int[] { 0, 1, 4, 1, 4, 5 }
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
                    face[j] = scale * CUBE_VERTICES[CUBE_FACES[i][j]];
                }
                faces[i] = new SolidFace(face);
            }
            return new Solid(faces);
        }
    }
}
