using Cardamom.Collections;

namespace Cardamom.Graphing
{
    public static class MinimalSpanningTree
    {
        public static IEnumerable<IGraphEdge> Compute<T>(IEnumerable<T> nodes) where T : IGraphNode
        {
            Heap<IGraphEdge, float> queue = new();
            var first = nodes.First();
            foreach (var edge in first.GetEdges())
            {
                queue.Push(edge, edge.Cost);
            }

            HashSet<T> open = new(nodes);
            open.Remove(first);
            while (open.Count > 0)
            {
                var edge = queue.Pop();
                if (open.Contains((T)edge.Start))
                {
                    yield return edge;
                    open.Remove((T)edge.Start);
                    foreach (var e in edge.Start.GetEdges())
                    {
                        queue.Push(e, e.Cost);
                    }
                }
                else if (open.Contains((T)edge.End))
                {
                    yield return edge;
                    open.Remove((T)edge.End);
                    foreach (var e in edge.End.GetEdges())
                    {
                        queue.Push(e, e.Cost);
                    }
                }
            }
        }
    }
}
