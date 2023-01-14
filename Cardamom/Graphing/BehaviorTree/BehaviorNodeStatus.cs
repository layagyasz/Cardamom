namespace Cardamom.Graphing.BehaviorTree
{
    public struct BehaviorNodeStatus
    {
        public bool Executed { get; set; }
        public bool Complete { get; set; }

        public BehaviorNodeStatus(bool executed, bool complete)
        {
            Executed = executed;
            Complete = complete;
        }

        public static BehaviorNodeStatus CreateNotRun()
        {
            return new BehaviorNodeStatus(false, false);
        }

        public static BehaviorNodeStatus CreateIncomplete()
        {
            return new BehaviorNodeStatus(true, false);
        }

        public static BehaviorNodeStatus CreateComplete()
        {
            return new BehaviorNodeStatus(true, true);
        }

        public override string ToString()
        {
            return string.Format("[BehaviorNodeStatus : Executed={0}, Complete={1}]", Executed, Complete);
        }
    }
}