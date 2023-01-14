namespace Cardamom.Graphing.BehaviorTree
{
    public class ClearBufferNode<TOut, TContext> : IBehaviorNode<BehaviorNodeStatus, TContext>
    {
        private readonly BufferNode<TOut, TContext> _node;

        public ClearBufferNode(BufferNode<TOut, TContext> node)
        {
            _node = node;
        }

        public BehaviorNodeStatus Execute(TContext context)
        {
            _node.Clear();
            return BehaviorNodeStatus.CreateComplete();
        }
    }
}