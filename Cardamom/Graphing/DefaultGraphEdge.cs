namespace Cardamom.Graphing
{
    public class DefaultGraphEdge : IGraphEdge
    {
        public IGraphNode Start { get; }
        public IGraphNode End { get; }
        public float Cost { get; }

        public DefaultGraphEdge(IGraphNode start, IGraphNode end, float cost)
        {
            Start = start;
            End = end;
            Cost = cost;
        }
    }
}
