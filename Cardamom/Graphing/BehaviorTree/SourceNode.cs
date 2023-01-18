using Cardamom.Utils.Suppliers;

namespace Cardamom.Graphing.BehaviorTree
{
    public class SourceNode<TOut, TContext> : ISupplierNode<TOut, TContext>
    {
        private ISupplier<TOut> _Source;

        public SourceNode(ISupplier<TOut> Source)
        {
            _Source = Source;
        }

        public static SourceNode<TOut, TContext> Wrap(TOut value)
        {
            return new SourceNode<TOut, TContext>(new ConstantSupplier<TOut>(value));
        }

        public static SourceNode<TOut, TContext> Wrap(Func<TOut> supplierFn)
        {
            return new SourceNode<TOut, TContext>(new FuncSupplier<TOut>(supplierFn));
        }

        public BehaviorNodeResult<TOut> Execute(TContext context)
        {
            return BehaviorNodeResult<TOut>.Complete(_Source.Get());
        }
    }
}