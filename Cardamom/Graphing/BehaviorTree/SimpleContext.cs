namespace Cardamom.Graphing.BehaviorTree
{
    public class SimpleContext
    {
        public bool DebugMode { get; set; }
        public bool RequireExecutionAcknowledgement { get; }

        public void Log(object source, string message, bool isDebug)
        {
            if (!isDebug || DebugMode)
            {
                Console.WriteLine("{0} [{1}] : {2}", DateTime.Now, source.GetType().Name, message);
            }
        }
    }
}