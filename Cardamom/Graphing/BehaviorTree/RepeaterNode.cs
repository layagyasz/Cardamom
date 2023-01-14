namespace Cardamom.Graphing.BehaviorTree
{
    public class RepeaterNode<TContext> : IBehaviorNode<BehaviorNodeStatus, TContext>
    {
        private readonly IBehaviorNode<BehaviorNodeStatus, TContext> _node;

        public RepeaterNode(IBehaviorNode<BehaviorNodeStatus, TContext> node)
        {
            _node = node;
        }

        public BehaviorNodeStatus Execute(TContext context)
        {
            var result = _node.Execute(context);
            if (!result.Executed)
            {
                return BehaviorNodeStatus.CreateNotRun();
            }
            if (!result.Complete)
            {
                return BehaviorNodeStatus.CreateIncomplete();
            }
            do
            {
                result = _node.Execute(context);
            }
            while (result.Complete);
            return BehaviorNodeStatus.CreateComplete();
        }
    }
}