namespace Cardamom.Graphing
{
    public static class MaxCostAssignment
    {
        public static IEnumerable<Tuple<TLeft, TRight>> Compute<TLeft, TRight>(
            IEnumerable<TLeft> left, IEnumerable<TRight> right) where TLeft : IGraphNode where TRight : IGraphNode
        {
            int numLeft = left.Count();
            int numRight = right.Count();
            var nodes = new Dictionary<object, HungarianNode>(numLeft + numRight);
            var leftNodes = new HungarianNode[numLeft];
            var rightNodes = new HungarianNode[numRight];

            int leftId = 0;
            foreach (var l in left)
            {
                HungarianNode H = new(l, leftId, numRight);
                leftNodes[leftId++] = H;
                nodes.Add(l, H);
            }
            int rightId = 0;
            foreach (var r in right)
            {
                HungarianNode h = new(r, rightId, numLeft);
                foreach (var edge in r.GetEdges())
                {
                    h.AddNeighbor(nodes[edge.End], edge.Cost);
                }
                rightNodes[rightId++] = h;
                nodes.Add(r, h);
            }
            foreach (var l in left)
            {
                var h = nodes[l];
                foreach (var edge in l.GetEdges())
                {
                    h.AddNeighbor(nodes[edge.End], edge.Cost);
                }
            }

            Assign(leftNodes, rightNodes);

            return leftNodes.Select(x => new Tuple<TLeft, TRight>((TLeft)x.Key, (TRight)x.Match!.Key));
        }

        private static void Assign(HungarianNode[] leftNodes, HungarianNode[] rightNodes)
        {
            int rounds = 0;
            Array.ForEach(leftNodes, l => l.Potential = l.Neighbors.Max());
            Array.ForEach(rightNodes, r => r.Potential = r.Neighbors.Max());
            Queue<HungarianNode> queue = new();
            while (rounds < leftNodes.Length)
            {
                rounds += Augment(queue, leftNodes, rightNodes);
            }
        }

        private static int Augment(Queue<HungarianNode> queue, HungarianNode[] leftNodes, HungarianNode[] rightNodes)
        {
            HungarianNode? root = null;
            foreach (var node in leftNodes)
            {
                if (node.Match == null)
                {
                    queue.Enqueue(node);
                    node.Parent = null;
                    node.Mark = true;
                    root = node;
                }
            }

            foreach (var node in rightNodes)
            {
                node.Slack = root!.Potential + node.Potential - root.GetCost(node);
                node.SlackNode = root;
            }

            var exposed = ExposePath(queue, rightNodes);

            if (exposed.Item2 == null)
            {
                UpdateLabels(leftNodes, rightNodes);
                exposed = ImproveLabeling(queue, rightNodes);
            }
            if (exposed.Item2 != null)
            {
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
            return 0;
        }

        private static void AddToTree(HungarianNode from, HungarianNode to, HungarianNode[] rightNodes)
        {
            to.Mark = true;
            to.Parent = from;
            foreach (var right in rightNodes)
            {
                if (to.Potential + right.Potential - to.GetCost(right) < right.Slack)
                {
                    right.Slack = to.Potential + right.Potential - to.GetCost(right);
                    right.SlackNode = to;
                }
            }
        }

        private static Tuple<HungarianNode?, HungarianNode?> ExposePath(
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
                            return new Tuple<HungarianNode?, HungarianNode?>(current, node);
                        }
                        else
                        {
                            node.Mark = true;
                            queue.Enqueue(node);
                            AddToTree(node, current, rightNodes);
                        }
                    }
                }
            }
            return new Tuple<HungarianNode?, HungarianNode?>(null, null);
        }

        private static Tuple<HungarianNode?, HungarianNode?> ImproveLabeling(
            Queue<HungarianNode> queue, HungarianNode[] rightNodes)
        {
            foreach (var node in rightNodes)
            {
                if (!node.Mark && Math.Abs(node.Slack) < float.Epsilon)
                {
                    if (node.Match == null)
                    {
                        return new Tuple<HungarianNode?, HungarianNode?>(node.SlackNode, node);
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
            return new Tuple<HungarianNode?, HungarianNode?>(null, null);
        }

        private static void UpdateLabels(HungarianNode[] leftNodes, HungarianNode[] rightNodes)
        {
            float delta = rightNodes.Min(r => !r.Mark ? r.Slack : float.PositiveInfinity);
            foreach (var node in leftNodes)
            {
                if (node.Mark)
                {
                    node.Potential -= delta;
                }
            }
            foreach (var node in rightNodes)
            {
                if (node.Mark)
                {
                    node.Potential += delta;
                }
                else
                {
                    node.Slack -= delta;
                }
            }
        }

        private class HungarianNode
        {
            public int Id { get; }
            public object Key { get; }
            public bool Mark { get; set; }
            public float Potential { get; set; }
            public float Slack { get; set; }
            public HungarianNode? Match { get; set; }
            public HungarianNode? SlackNode { get; set; }
            public HungarianNode? Parent { get; set; }
            public float[] Neighbors { get; }

            public HungarianNode(object key, int id, int size)
            {
                Id = id;
                Key = key;
                Neighbors = new float[size];
            }

            public void AddNeighbor(HungarianNode node, float cost)
            {
                Neighbors[node.Id] = cost;
            }

            public float GetCost(HungarianNode node)
            {
                return Neighbors[node.Id];
            }
        }
    }
}
