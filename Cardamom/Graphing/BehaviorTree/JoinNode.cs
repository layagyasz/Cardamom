namespace Cardamom.Graphing.BehaviorTree
{
    public class JoinNode<TLeft, TRight, TContext> : ISupplierNode<List<Tuple<TLeft, TRight>>, TContext>
    {
        private readonly ISupplierNode<List<TLeft>, TContext> _leftNode;
        private readonly ISupplierNode<List<TRight>, TContext> _rightNode;
        private readonly Func<TLeft, TRight, bool> _joinFn;

        public JoinNode(
            ISupplierNode<List<TLeft>,  TContext> leftNode, 
            ISupplierNode<List<TRight>, TContext> rightNode, 
            Func<TLeft, TRight, bool> joinFn)
        {
            _leftNode = leftNode;
            _rightNode = rightNode;
            _joinFn = joinFn;
        }

        public BehaviorNodeResult<List<Tuple<TLeft, TRight>>> Execute(TContext context)
        {
            var left = _leftNode.Execute(context);
            var right = _rightNode.Execute(context);

            if (!left.Status.Executed && !right.Status.Executed)
            {
                return BehaviorNodeResult<List<Tuple<TLeft, TRight>>>.NotRun();
            }
            if (!left.Status.Complete || !right.Status.Complete)
            {
                return BehaviorNodeResult<List<Tuple<TLeft, TRight>>>.Incomplete();
            }

            var result = new List<Tuple<TLeft, TRight>>();
            foreach (var l in left.Result!)
            {
                foreach (var r in right.Result!)
                {
                    if (_joinFn(l, r))
                    {
                        result.Add(new Tuple<TLeft, TRight>(l, r));
                    }
                }
            }

            return BehaviorNodeResult<List<Tuple<TLeft, TRight>>>.Complete(result);
        }
    }
}