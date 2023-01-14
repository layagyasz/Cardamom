namespace Cardamom.Graphing.BehaviorTree
{
    public class ChainNode<TOut, TContext> : ISupplierNode<TOut, TContext>
    {
        private readonly IBehaviorNode<BehaviorNodeStatus, TContext> _left;
        private readonly ISupplierNode<TOut, TContext> _right;

        public ChainNode(IBehaviorNode<BehaviorNodeStatus, TContext> left, ISupplierNode<TOut, TContext> right)
        {
            _left = left;
            _right = right;
        }

        public BehaviorNodeResult<TOut> Execute(TContext context)
        {
            var result = _left.Execute(context);
            if (!result.Executed)
            {
                return BehaviorNodeResult<TOut>.NotRun();
            }
            if (!result.Complete)
            {
                return BehaviorNodeResult<TOut>.Incomplete();
            }

            return _right.Execute(context);
        }
    }
}