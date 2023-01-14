namespace Cardamom.Graphing.BehaviorTree
{
    public class CapNode<TIn, TContext> : IBehaviorNode<BehaviorNodeStatus, TContext>
    {
        private readonly ISupplierNode<TIn, TContext> _node;

        public CapNode(ISupplierNode<TIn, TContext> node)
        {
            _node = node;
        }

        public BehaviorNodeStatus Execute(TContext context)
        {
            return _node.Execute(context).Status;
        }
    }
}
