using Cardamom.Collections;

namespace Cardamom.Graphing
{
    public static class MinimalCostAssignment
    {
        public static IEnumerable<Tuple<TLeft, TRight>> ComputeGreedy<TLeft, TRight>(
             IEnumerable<TLeft> left, IEnumerable<TRight> right, Func<TLeft, TRight, float> costFn)
             where TLeft : notnull where TRight : notnull
        {
            (var leftNodes, var rightNodes) =
                BipartiteGraph.Generate(left, right, new GraphGenerator<TLeft, TRight>(costFn));
            AssignGreedy(leftNodes, rightNodes);

            return leftNodes.Select(x => new Tuple<TLeft, TRight>((TLeft)x.Value, (TRight)x.Match!.Value)).ToList();
        }

        public static IEnumerable<Tuple<TLeft, TRight>> ComputeOptimum<TLeft, TRight>(
            IEnumerable<TLeft> left, IEnumerable<TRight> right, Func<TLeft, TRight, float> costFn) 
            where TLeft : notnull where TRight : notnull
        {
            (var leftNodes, var rightNodes) = 
                BipartiteGraph.Generate(left, right, new GraphGenerator<TLeft, TRight>(costFn));
            AssignHungarian(leftNodes, rightNodes);

            return leftNodes.Select(x => new Tuple<TLeft, TRight>((TLeft)x.Value, (TRight)x.Match!.Value)).ToList();
        }

        private static void AssignGreedy(HungarianNode[] leftNodes, HungarianNode[] rightNodes)
        {
            foreach (var left in leftNodes)
            {
                left.Match = rightNodes.Where(r => r.Match == null).ArgMin(left.GetCost);
                left.Match!.Match = left;
            }
        }

        private static void AssignHungarian(HungarianNode[] leftNodes, HungarianNode[] rightNodes)
        {
            int rounds = 0;
            Array.ForEach(leftNodes, l => l.Potential = l.Costs.Min());
            Array.ForEach(rightNodes,r => r.Potential = leftNodes.Min(l => r.GetCost(l) - l.Potential));
            foreach (var left in leftNodes)
            {
                foreach (var right in rightNodes)
                {
                    if (Math.Abs(left.GetCost(right) - left.Potential - right.Potential) < float.Epsilon
                        && right.Match == null)
                    {
                        left.Match = right;
                        right.Match = left;
                        ++rounds;
                        break;
                    }
                }
            }
            while (rounds < leftNodes.Length)
            {
                rounds += Augment(leftNodes, rightNodes);
            }
        }

        private static int Augment(HungarianNode[] leftNodes, HungarianNode[] rightNodes)
        {
            Queue<HungarianNode> queue = new();
            foreach (var node in leftNodes)
            {
                node.Mark = false;
            }
            var root = leftNodes.First(x => x.Match == null);
            queue.Enqueue(root);
            root.Mark = true;
            root.Parent = null;

            foreach (var node in rightNodes)
            {
                node.Mark = false;
                node.Slack = root.GetCost(node) - root.Potential - node.Potential;
                node.SlackNode = root;
            }

            (HungarianNode?, HungarianNode?) exposed;
            do
            {
                exposed = ExposePath(queue, rightNodes);
                if (exposed.Item2 == null)
                {
                    UpdatePotentials(leftNodes, rightNodes);
                    exposed = ImprovePotentials(queue, rightNodes);
                }
                else
                {
                }
            }
            while (exposed.Item2 == null);

            var currentRight = exposed.Item2;
            var currentLeft = exposed.Item1;
            while (currentLeft != null && currentRight != null)
            {
                var tempRight = currentLeft.Match;

                currentRight.Match = currentLeft;
                currentLeft.Match = currentRight;

                currentRight = tempRight;
                currentLeft = currentLeft.Parent;
            }
            return 1;
        }

        private static void AddToTree(HungarianNode from, HungarianNode to, HungarianNode[] rightNodes)
        {
            to.Mark = true;
            to.Parent = from;
            foreach (var right in rightNodes)
            {
                if (to.GetCost(right) - to.Potential - right.Potential < right.Slack)
                {
                    right.Slack = to.GetCost(right) - to.Potential - right.Potential;
                    right.SlackNode = to;
                }
            }
        }

        private static (HungarianNode?, HungarianNode?) ExposePath(
            Queue<HungarianNode> queue, HungarianNode[] rightNodes)
        {
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var node in rightNodes)
                {
                    if (Math.Abs(current.GetCost(node) - current.Potential - node.Potential) 
                        < float.Epsilon && !node.Mark)
                    {
                        if (node.Match == null)
                        {
                            return (current, node);
                        }
                        else
                        {
                            node.Mark = true;
                            queue.Enqueue(node.Match);
                            AddToTree(current, node.Match, rightNodes);
                        }
                    }
                }
            }
            return (null, null);
        }

        private static (HungarianNode?, HungarianNode?) ImprovePotentials(
            Queue<HungarianNode> queue, HungarianNode[] rightNodes)
        {
            foreach (var node in rightNodes)
            {
                if (!node.Mark && Math.Abs(node.Slack) < float.Epsilon)
                {
                    if (node.Match == null)
                    {
                        return (node.SlackNode, node);
                    }
                    else
                    {
                        node.Mark = true;
                        if (!node.Match.Mark)
                        {
                            queue.Enqueue(node.Match);
                            AddToTree(node.SlackNode!, node.Match, rightNodes);
                        }
                    }
                }
            }
            return (null, null);
        }

        private static void UpdatePotentials(HungarianNode[] leftNodes, HungarianNode[] rightNodes)
        {
            float delta = rightNodes.Min(r => !r.Mark ? r.Slack : float.MaxValue);
            foreach (var node in leftNodes)
            {
                if (node.Mark)
                {
                    node.Potential += delta;
                }
            }
            foreach (var node in rightNodes)
            {
                if (node.Mark)
                {
                    node.Potential -= delta;
                }
                else
                {
                    node.Slack -= delta;
                }
            }
        }

        private class GraphGenerator<TLeft, TRight> 
            : IBipartiteGraphGenerator<TLeft, HungarianNode, TRight, HungarianNode> 
            where TLeft: notnull where TRight : notnull
        {
            private readonly Func<TLeft, TRight, float> _costFn;

            public GraphGenerator(Func<TLeft, TRight, float> costFn)
            {
                _costFn = costFn;
            }

            public HungarianNode GenerateNode(int id, TLeft value, int numNeighbors)
            {
                return new HungarianNode(id, value, numNeighbors);
            }

            public HungarianNode GenerateNode(int id, TRight value, int numNeighbors)
            {
                return new HungarianNode(id, value, numNeighbors);
            }

            public float GetLefthandCost(TLeft left, TRight right)
            {
                return _costFn(left, right);
            }

            public float GetRighthandCost(TLeft left, TRight right)
            {
                return _costFn(left, right);
            }
        }

        private class HungarianNode : IBipartiteNode
        {
            public int Id { get; }
            public object Value { get; }
            public bool Mark { get; set; }
            public float Potential { get; set; }
            public float Slack { get; set; }
            public HungarianNode? Match { get; set; }
            public HungarianNode? SlackNode { get; set; }
            public HungarianNode? Parent { get; set; }
            public float[] Costs { get; }

            public HungarianNode(int id, object value, int numNeighbors)
            {
                Id = id;
                Value = value;
                Costs = new float[numNeighbors];
            }

            public void SetCost(IBipartiteNode other, float cost)
            {
                Costs[other.Id] = cost;
            }

            public float GetCost(IBipartiteNode other)
            {
                return Costs[other.Id];
            }
        }
    }
}
