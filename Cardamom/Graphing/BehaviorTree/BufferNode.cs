namespace Cardamom.Graphing.BehaviorTree
{
    public class BufferNode<TOut, TContext> : ISupplierNode<TOut, TContext>
    {
        private readonly ISupplierNode<TOut, TContext> _node;

        private bool _present;
        private BehaviorNodeResult<TOut>? _result;

        public BufferNode(ISupplierNode<TOut, TContext> node)
        {
            _node = node;
        }

        public BehaviorNodeResult<TOut> Execute(TContext context)
        {
            if (_present)
            {
                return _result!;
            }
            _result = _node.Execute(context);
            _present = true;
            return _result;
        }

        public void Clear()
        {
            _present = false;
        }
    }
}