using Cardamom.Collections;

namespace Cardamom.Graphing
{
    public static class ShortestPath
    {
        public static Stack<IGraphEdge> FindPath<TNode>(
            TNode start, TNode end, Func<TNode, TNode, float> heuristicFn, Func<IGraphEdge, bool> admissibilityFn)
            where TNode : IGraphNode
        {
            Dictionary<TNode, WrapperNode<TNode>> nodes = new();
            Heap<WrapperNode<TNode>, float> open = new();
            var startNode = new WrapperNode<TNode>(start, 0);
            open.Push(startNode, 0);

            WrapperNode<TNode> current;
            while (open.Count > 0)
            {
                current = open.Pop();
                if (Equals(current.Node, end))
                {
                    break;
                }
                current.Open = false;
                foreach (var edge in current.Node.GetEdges())
                {
                    if (admissibilityFn(edge))
                    {
                        var cost = current.Cost + edge.Cost;
                        if (!nodes.TryGetValue((TNode)edge.End, out var other))
                        {
                            other = new WrapperNode<TNode>((TNode)edge.End, float.PositiveInfinity);
                            nodes.Add((TNode)edge.End, other);
                        }
                        if (cost < other.Cost)
                        {
                            if (other.Open)
                            {
                                open.Remove(other);
                            }
                            else
                            {
                                other.Open = true;
                            }
                            other.Update(cost, current, edge);
                            open.Push(other, cost + heuristicFn((TNode)edge.End, end));
                        }
                    }
                }
            }

            var path = new Stack<IGraphEdge>();
            current = nodes[end];
            while (!Equals(current.Node, start))
            {
                path.Push(current.ParentEdge!);
                current = current.Parent!;
            }

            return path;
        }

        private class WrapperNode<TNode>
        {
            public TNode Node { get; }
            public bool Open { get; set; }
            public float Cost { get; private set; }
            public WrapperNode<TNode>? Parent { get; private set; }
            public IGraphEdge? ParentEdge { get; private set; }

            public WrapperNode(TNode node, float cost)
            {
                Node = node;
                Cost = cost;
            }

            public void Update(float cost, WrapperNode<TNode> parent, IGraphEdge parentEdge)
            {
                Cost = cost;
                Parent = parent;
                ParentEdge = parentEdge;
            }
        }
    }
}
