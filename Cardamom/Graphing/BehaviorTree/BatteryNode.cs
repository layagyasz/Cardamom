namespace Cardamom.Graphing.BehaviorTree
{
    public class BatteryNode<TContext> : CollectionNode<BehaviorNodeStatus, TContext>
    {
        public override BehaviorNodeStatus Execute(TContext context)
        {
            bool run = false;
            bool complete = false;
            foreach (var node in this)
            {
                var result = node.Execute(context);
                if (result.Executed)
                {
                    run = true;
                }
                if (result.Complete)
                {
                    complete = true;
                }
            }
            if (complete)
            {
                return BehaviorNodeStatus.CreateComplete();
            }
            if (run)
            {
                return BehaviorNodeStatus.CreateIncomplete();
            }
            return BehaviorNodeStatus.CreateNotRun();
        }
    }
}
