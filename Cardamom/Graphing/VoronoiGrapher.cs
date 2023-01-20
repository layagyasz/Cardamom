using DelaunayTriangulator;
using OpenTK.Mathematics;

namespace Cardamom.Graphing
{
    public static class VoronoiGrapher
    {
        public class VoronoiNeighborsResult
        {
            public List<List<int>> Neighbors { get; set; }
            public List<int> EdgeIndices { get; set; }

            public VoronoiNeighborsResult(List<List<int>> neighbors, List<int> edgeIndices)
            {
                Neighbors = neighbors;
                EdgeIndices = edgeIndices;
            }
        }

        public static Vector2 GetCircumcenter(Vector2 a, Vector2 b, Vector2 c)
        {
            var points = new List<Vertex>()
            {
                new Vertex(a.X, a.Y),
                new Vertex(b.X, b.Y),
                new Vertex(c.X, c.Y)
            };
            var triad = new Triad(0, 1, 2);
            triad.FindCircumcirclePrecisely(points);
            return new(triad.circumcircleX, triad.circumcircleY);
        }

        public static List<Triad> GetTriangulation(List<Vertex> vertices)
        {
            Triangulator triangulator = new();
            return triangulator.Triangulation(vertices);
        }

        public static VoronoiNeighborsResult GetNeighbors(List<Vertex> vertices, List<Triad> triads)
        {
            List<WrapperNode> wrapperNodes = vertices.Select(x => new WrapperNode()).ToList();
            foreach (var triad in triads)
            {
                wrapperNodes[triad.a].Triads.Add(triad);
                wrapperNodes[triad.b].Triads.Add(triad);
                wrapperNodes[triad.c].Triads.Add(triad);
            }

            List<List<int>> allNeighbors = new();
            List<int> edgeIndices = new();
            for (int i = 0; i < vertices.Count; ++i)
            {
                List<int> neighbors = new();
                for (int j = 0; j < wrapperNodes[i].Triads.Count; ++j)
                {
                    Triad triad = wrapperNodes[i].Triads[j];
                    if (i == triad.a)
                    {
                        neighbors.Add(triad.b);
                        neighbors.Add(triad.c);
                        if (triad.ab == -1 || triad.ac == -1)
                        {
                            neighbors.Add(-1);
                            edgeIndices.Add(i);
                        }
                    }
                    else if (i == triad.b)
                    {
                        neighbors.Add(triad.a);
                        neighbors.Add(triad.c);
                        if (triad.ab == -1 || triad.bc == -1)
                        {
                            neighbors.Add(-1);
                            edgeIndices.Add(i);
                        }
                    }
                    else
                    {
                        neighbors.Add(triad.a);
                        neighbors.Add(triad.b);
                        if (triad.bc == -1 || triad.ac == -1)
                        {
                            neighbors.Add(-1);
                            edgeIndices.Add(i);
                        }
                    }
                }
                allNeighbors.Add(neighbors.Distinct().ToList());
            }

            return new VoronoiNeighborsResult(allNeighbors, edgeIndices.Distinct().ToList());
        }

        private class WrapperNode
        {
            public List<Triad> Triads { get; } = new();
        }
    }
}