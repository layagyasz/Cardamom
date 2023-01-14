namespace Cardamom.Graphing.BehaviorTree
{
    public class ExploderNode<TOut, TContext> : ISupplierNode<TOut, TContext>
    {
        private readonly ISupplierNode<List<TOut>, TContext> _node;

        private List<TOut>? _values;

        public ExploderNode(ISupplierNode<List<TOut>, TContext> node)
        {
            _node = node;
        }

        public BehaviorNodeResult<TOut> Execute(TContext context)
        {
            if (_values == null)
            {
                var result = _node.Execute(context);
                if (!result.Status.Executed || !result.Status.Complete)
                {
                    return BehaviorNodeResult<TOut>.NotRun();
                }
                _values = result.Result!.ToList();
            }
            if (_values.Count == 0)
            {
                _values = null;
                return BehaviorNodeResult<TOut>.NotRun();
            }
            var value = _values[0];
            _values.RemoveAt(0);
            return BehaviorNodeResult<TOut>.Complete(value);
        }
    }
}