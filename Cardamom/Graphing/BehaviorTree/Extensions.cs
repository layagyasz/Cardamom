namespace Cardamom.Graphing.BehaviorTree
{
    public static class Extensions
    {
        public static TransformNode<TIn, TOut, TContext> Transform<TIn, TOut, TContext>(
            this ISupplierNode<TIn, TContext> node, Func<TIn, TOut> transformFn)
        {
            return new TransformNode<TIn, TOut, TContext>(node, transformFn);
        }

        public static TransformNode<List<TIn>, List<TOut>, TContext> TransformAll<TIn, TOut, TContext>(
            this ISupplierNode<List<TIn>, TContext> node, Func<TIn, TOut> transformFn)
        {
            return new TransformNode<List<TIn>, List<TOut>, TContext>(node, x => x.Select(transformFn).ToList());
        }

        public static ISupplierNode<TOut, TContext> Check<TOut, TContext>(
            this ISupplierNode<TOut, TContext> node, Predicate<TOut> checkFn)
        {
            return new CheckNode<TOut, TContext>(node, (x, y) => checkFn(x));
        }

        public static ISupplierNode<TOut, TContext> Check<TOut, TContext>(
            this ISupplierNode<TOut, TContext> node, Func<TOut, TContext, bool> checkFn)
        {
            return new CheckNode<TOut, TContext>(node, checkFn);
        }

        public static ISupplierNode<TOut, TContext> Explode<TOut, TContext>(
            this ISupplierNode<List<TOut>, TContext> node)
        {
            return new ExploderNode<TOut, TContext>(node);
        }

        public static ISupplierNode<List<TOut>, TContext> Implode<TOut, TContext>(
            this ISupplierNode<TOut, TContext> node)
        {
            return new TransformNode<TOut, List<TOut>, TContext>(node, x => new List<TOut>() { x });
        }

        public static IBehaviorNode<BehaviorNodeStatus, TContext> Repeat<TContext>(
            this IBehaviorNode<BehaviorNodeStatus, TContext> node)
        {
            return new RepeaterNode<TContext>(node);
        }

        public static IBehaviorNode<BehaviorNodeStatus, TContext> Cap<TIn, TContext>(
            this ISupplierNode<TIn, TContext> node)
        {
            return new CapNode<TIn, TContext>(node);
        }

        public static ISupplierNode<TOut, TContext> Adapt<TOut, TContext>(
            this IBehaviorNode<BehaviorNodeResult<TOut>, TContext> node)
        {
            return new AdaptorNode<TOut, TContext>(node);
        }

        public static ISupplierNode<TOut, TContext> AndThen<TOut, TContext>(
            this IBehaviorNode<BehaviorNodeStatus, TContext> node, ISupplierNode<TOut, TContext> andThenNode)
        {
            return new ChainNode<TOut, TContext>(node, andThenNode);
        }

        public static ISupplierNode<TIn, TContext> UpdateContext<TIn, TContext>(
            this ISupplierNode<TIn, TContext> node, Action<TIn, TContext> updateFn)
        {
            return new UpdateContextNode<TIn, TContext>(node, updateFn);
        }

        public static BufferNode<TOut, TContext> Buffer<TOut, TContext>(this ISupplierNode<TOut, TContext> node)
        {
            return new BufferNode<TOut, TContext>(node);
        }

        public static IBehaviorNode<BehaviorNodeStatus, TContext> ClearBuffer<TOut, TContext>(
            this BufferNode<TOut, TContext> node)
        {
            return new ClearBufferNode<TOut, TContext>(node);
        }

        public static ISupplierNode<TOut, TContext> Recompute<TOut, TContext>(
            this BufferNode<TOut, TContext> node)
        {
            return node.ClearBuffer().AndThen(node);
        }
    }
}