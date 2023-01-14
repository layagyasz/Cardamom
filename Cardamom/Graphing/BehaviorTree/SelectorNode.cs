namespace Cardamom.Graphing.BehaviorTree
{
    public class SelectorNode<TOut, TContext> : CollectionNode<TOut, TContext>
    {
        private readonly Func<TOut, bool> _selectorFn;
        private readonly TOut _default;

        public SelectorNode(Func<TOut, bool> selectorFn, TOut @default)
        {
            _selectorFn = selectorFn;
            _default = @default;
        }

        public override TOut Execute(TContext context)
        {
            foreach (var node in this)
            {
                var result = node.Execute(context);
                if (_selectorFn(result))
                {
                    return result;
                }
            }
            return _default;
        }
    }
}