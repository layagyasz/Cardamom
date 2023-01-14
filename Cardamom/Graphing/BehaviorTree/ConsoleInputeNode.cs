namespace Cardamom.Graphing.BehaviorTree
{
    public class ConsoleInputNode<TContext> : ISupplierNode<Dictionary<string, string>, TContext>
    {
        public List<string> Fields { get; }

        public ConsoleInputNode(params string[] fields)
        {
            this.Fields = fields.ToList();
        }

        public BehaviorNodeResult<Dictionary<string, string>> Execute(TContext context)
        {
            var dict = new Dictionary<string, string>();
            foreach (var field in Fields)
            {
                Console.Write("{0}: ", field);
                dict.Add(field, Console.ReadLine()!);
            }

            return BehaviorNodeResult<Dictionary<string, string>>.Complete(dict);
        }
    }
}