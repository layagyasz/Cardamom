namespace Cardamom.Graphing
{
    public interface IBipartiteGraphGenerator<TLeft, TLeftNode, TRight, TRightNode>
    {
        TLeftNode GenerateNode(int id, TLeft value, int numNeighbors);
        TRightNode GenerateNode(int id, TRight value, int numNeighbors);
        float GetLefthandCost(TLeft left, TRight right);
        float GetRighthandCost(TLeft left, TRight right);
    }
}
