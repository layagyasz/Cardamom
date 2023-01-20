namespace Cardamom.Graphing
{
    public interface IGraphEdge
    {
        IGraphNode Start { get; }
        IGraphNode End { get; }
        float Cost { get; }
    }
}
