namespace Cardamom.Graphing.BehaviorTree
{
    public class CheckNode<TOut, TContext> : ISupplierNode<TOut, TContext>
    {
        private readonly ISupplierNode<TOut, TContext> _node;
        private readonly Func<TOut, TContext, bool> _checkFn;

        public CheckNode(ISupplierNode<TOut, TContext> node, Func<TOut, TContext, bool> checkFn)
        {
            _node = node;
            _checkFn = checkFn;
        }

        public BehaviorNodeResult<TOut> Execute(TContext context)
        {
            var result = _node.Execute(context);
            if (result.Status.Complete)
            {
                if (_checkFn(result.Result!, context))
                {
                    return result;
                }
                return BehaviorNodeResult<TOut>.Incomplete();
            }
            return result;
        }
    }
}