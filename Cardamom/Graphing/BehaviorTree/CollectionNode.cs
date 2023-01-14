using System.Collections;

namespace Cardamom.Graphing.BehaviorTree
{
    public abstract class CollectionNode<TOut, TContext> :
        IBehaviorNode<TOut, TContext>, IEnumerable<IBehaviorNode<TOut, TContext>>
    {
        private List<IBehaviorNode<TOut, TContext>> _nodes = new();

        public IEnumerator<IBehaviorNode<TOut, TContext>> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IBehaviorNode<TOut, TContext> Node)
        {
            _nodes.Add(Node);
        }

        public abstract TOut Execute(TContext Context);
    }
}