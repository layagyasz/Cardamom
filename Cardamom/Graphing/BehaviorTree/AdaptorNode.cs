namespace Cardamom.Graphing.BehaviorTree
{
    public class AdaptorNode<TOut, TContext> : ISupplierNode<TOut, TContext>
    {
        private readonly IBehaviorNode<BehaviorNodeResult<TOut>, TContext> _node;

        public AdaptorNode(IBehaviorNode<BehaviorNodeResult<TOut>, TContext> node)
        {
            _node = node;
        }

        public BehaviorNodeResult<TOut> Execute(TContext context)
        {
            return _node.Execute(context);
        }
    }
}