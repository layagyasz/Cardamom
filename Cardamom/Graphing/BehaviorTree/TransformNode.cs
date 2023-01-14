namespace Cardamom.Graphing.BehaviorTree
{
    public class TransformNode<TIn, TOut, TContext> : ISupplierNode<TOut, TContext>
    {
        private readonly ISupplierNode<TIn, TContext> _node;
        private readonly Func<TIn, TOut> _transformFn;

        public TransformNode(ISupplierNode<TIn, TContext> node, Func<TIn, TOut> transformFn)
        {
            _node = node;
            _transformFn = transformFn;
        }

        public BehaviorNodeResult<TOut> Execute(TContext context)
        {
            var result = _node.Execute(context);
            if (!result.Status.Executed)
            {
                return BehaviorNodeResult<TOut>.NotRun();
            }
            if (!result.Status.Complete)
            {
                return BehaviorNodeResult<TOut>.Incomplete();
            }
            return BehaviorNodeResult<TOut>.Complete(_transformFn(result.Result!));
        }
    }
}