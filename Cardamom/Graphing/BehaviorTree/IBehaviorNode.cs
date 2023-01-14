namespace Cardamom.Graphing.BehaviorTree
{
    public interface IBehaviorNode<TOut, TContext>
    {
        TOut Execute(TContext context);
    }
}
