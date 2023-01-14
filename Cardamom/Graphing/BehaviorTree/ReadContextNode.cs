namespace Cardamom.Graphing.BehaviorTree
{
    public class ReadContextNode<TOut, TContext> : ISupplierNode<TOut, TContext>
    {
        private readonly Func<TContext, TOut> _readFn;

        public ReadContextNode(Func<TContext, TOut> readFn)
        {
            _readFn = readFn;
        }

        public static ReadContextNode<TOut, TContext> Wrap(Func<TContext, TOut> readFn)
        {
            return new ReadContextNode<TOut, TContext>(readFn);
        }

        public BehaviorNodeResult<TOut> Execute(TContext context)
        {
            return BehaviorNodeResult<TOut>.Complete(_readFn(context));
        }
    }
}
