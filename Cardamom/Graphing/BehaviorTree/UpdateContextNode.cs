namespace Cardamom.Graphing.BehaviorTree
{
    public class UpdateContextNode<TIn, TContext> : ISupplierNode<TIn, TContext>
    {
        private readonly ISupplierNode<TIn, TContext> _node;
        private readonly Action<TIn, TContext> _updateFn;

        public UpdateContextNode(ISupplierNode<TIn, TContext> node, Action<TIn, TContext> updateFn)
        {
            _node = node;
            _updateFn = updateFn;
        }

        public BehaviorNodeResult<TIn> Execute(TContext context)
        {
            var result = _node.Execute(context);
            if (result.Status.Complete)
            {
                _updateFn(result.Result!, context);
            }
            return result;
        }
    }
}
