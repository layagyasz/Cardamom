namespace Cardamom.Graphing.BehaviorTree
{
    public class CheckContextNode<TContext> : IBehaviorNode<BehaviorNodeStatus, TContext>
    {
        private readonly Func<TContext, bool> _checkFn;

        public CheckContextNode(Func<TContext, bool> checkFn)
        {
            _checkFn = checkFn;
        }

        public BehaviorNodeStatus Execute(TContext context)
        {
            return _checkFn(context) ? BehaviorNodeStatus.CreateComplete() : BehaviorNodeStatus.CreateNotRun();
        }
    }
}