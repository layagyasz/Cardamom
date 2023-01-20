namespace Cardamom.Graphing
{
    public interface IGraphNode
    {
        IEnumerable<IGraphEdge> GetEdges();
    }
}
