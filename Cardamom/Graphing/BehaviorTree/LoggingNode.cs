namespace Cardamom.Graphing.BehaviorTree
{
    public abstract class LoggingNode<TOut, TContext> : IBehaviorNode<TOut, TContext> where TContext : SimpleContext
    {
        public string Name { get; }

        public LoggingNode(string name)
        {
            Name = name;
        }

        public void Log(TContext context)
        {
            context.Log(this, Name, true);
        }

        public abstract TOut Execute(TContext context);
    }
}