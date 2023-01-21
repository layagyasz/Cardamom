namespace Cardamom.Graphing
{
    public interface IBipartiteNode
    {
        int Id { get; }
        object Value { get; }
        public float GetCost(IBipartiteNode other);
        public void SetCost(IBipartiteNode other, float cost);
    }
}
