using MathNet.Numerics;

namespace Cardamom.Graphing
{
    public static class BipartiteGraph
    {
        public static (TLeftNode[], TRightNode[]) Generate<TLeft, TLeftNode, TRight, TRightNode>(
            IEnumerable<TLeft> left, 
            IEnumerable<TRight> right, 
            IBipartiteGraphGenerator<TLeft, TLeftNode, TRight, TRightNode> generator)
            where TLeft : notnull 
            where TRight : notnull
            where TLeftNode : IBipartiteNode 
            where TRightNode : IBipartiteNode
        {
            int numLeft = left.Count();
            int numRight = right.Count();

            int leftId = 0;
            var leftNodes = new TLeftNode[numLeft];
            foreach (var value in left)
            {
                var node = generator.GenerateNode(leftId, value, numRight);
                leftNodes[leftId++] = node;
            }

            int rightId = 0;
            var rightNodes = new TRightNode[numRight];
            foreach (var value in right)
            {
                var node = generator.GenerateNode(rightId, value, numLeft);
                foreach (var other in leftNodes)
                {
                    node.SetCost(other, generator.GetRighthandCost((TLeft)other.Value, value));
                    other.SetCost(node, generator.GetLefthandCost((TLeft)other.Value, value));
                }
                rightNodes[rightId++] = node;
            }

            return (leftNodes, rightNodes);
        }
    }
}
