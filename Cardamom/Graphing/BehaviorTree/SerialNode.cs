namespace Cardamom.Graphing.BehaviorTree
{
    public class SerialNode<TContext> : CollectionNode<BehaviorNodeStatus, TContext>
    {
        public override BehaviorNodeStatus Execute(TContext context)
        {
            bool run = false;
            foreach (var node in this)
            {
                var result = node.Execute(context);
                if (!result.Executed && !run)
                {
                    return BehaviorNodeStatus.CreateNotRun();
                }
                else if (!result.Complete)
                {
                    return BehaviorNodeStatus.CreateIncomplete();
                }
                else
                {
                    run = true;
                }
            }
            return BehaviorNodeStatus.CreateComplete();
        }
    }
}