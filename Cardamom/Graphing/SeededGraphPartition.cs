using Cardamom.Collections;

namespace Cardamom.Graphing
{
    public static class SeededGraphPartition
    {
        public interface ISeed<TNode>
        {
            public TNode Origin { get; }
            public float InitialCost { get; }
            public float GetPenalty(TNode node);
        }

        public class Partition<TSeed, TNode>
        {
            public TSeed Seed { get; }
            public List<TNode> Nodes { get; }

            public Partition(TSeed seed, List<TNode> nodes)
            {
                Seed = seed;
                Nodes = nodes;
            }
        }

        public static IEnumerable<Partition<TSeed, TNode>> Compute<TSeed, TNode>(IEnumerable<TSeed> seeds) 
            where TNode : IGraphNode where TSeed : ISeed<TNode>
        {
            Dictionary<TNode, WrapperNode<TSeed, TNode>> nodes = new();

            Heap<WrapperNode<TSeed, TNode>, float> open = new();
            HashSet<WrapperNode<TSeed, TNode>> openSet = new();
            foreach (var seed in seeds)
            {
                if (!nodes.TryGetValue(seed.Origin, out var node))
                {
                    node = new WrapperNode<TSeed, TNode>(seed.Origin, seed.InitialCost, seed);
                    nodes.Add(seed.Origin, node);
                }
                open.Push(node, 0);
                openSet.Add(node);
            }
            while (open.Count > 0)
            {
                var current = open.Pop();
                openSet.Remove(current);
                foreach (var edge in current.Node.GetEdges())
                {
                    var cost = current.Cost + edge.Cost;
                    var neighbor = (TNode)edge.End;
                    if (!nodes.TryGetValue(neighbor, out var neighborNode))
                    {
                        neighborNode = new WrapperNode<TSeed, TNode>(neighbor, float.PositiveInfinity, current.Seed);
                        nodes.Add(neighbor, neighborNode);
                    }
                    if (cost < neighborNode.Cost)
                    {
                        if (openSet.Contains(neighborNode))
                        {
                            open.Remove(neighborNode);
                        }
                        else
                        {
                            openSet.Add(neighborNode);
                        }
                        neighborNode.Update(cost, current.Seed);
                        open.Push(neighborNode, cost);
                    }
                }
            }

            return nodes.Values
                .GroupBy(x => x.Seed)
                .Select(x => new Partition<TSeed, TNode>(x.Key, x.Select(y => y.Node).ToList()));
        }

        private class WrapperNode<TSeed, TNode>
        {
            public TNode Node { get; }
            public float Cost { get; private set; }
            public TSeed Seed { get; private set; }

            public WrapperNode(TNode node, float cost, TSeed seed)
            {
                Node = node;
                Cost = cost;
                Seed = seed;
            }

            public void Update(float cost, TSeed seed)
            {
                Cost = cost;
                Seed = seed;
            }
        }
    }
}
