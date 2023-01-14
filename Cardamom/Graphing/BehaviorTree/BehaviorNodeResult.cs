namespace Cardamom.Graphing.BehaviorTree
{
    public class BehaviorNodeResult<T>
    {
        public T? Result { get; set; }
        public BehaviorNodeStatus Status { get; set; }

        private BehaviorNodeResult(T? result, BehaviorNodeStatus status)
        {
            Result = result;
            Status = status;
        }

        public static BehaviorNodeResult<T> Complete(T result)
        {
            return new BehaviorNodeResult<T>(result, BehaviorNodeStatus.CreateComplete());
        }

        public static BehaviorNodeResult<T> NotRun()
        {
            return new BehaviorNodeResult<T>(default, BehaviorNodeStatus.CreateNotRun());
        }

        public static BehaviorNodeResult<T> Incomplete()
        {
            return new BehaviorNodeResult<T>(default, BehaviorNodeStatus.CreateIncomplete());
        }
    }
}
