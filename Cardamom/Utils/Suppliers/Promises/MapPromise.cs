namespace Cardamom.Utils.Suppliers.Promises
{
    public class MapPromise<TIn, TOut> : IPromise<TOut>
    {
        public EventHandler<EventArgs>? Canceled { get; set; }
        public EventHandler<EventArgs>? Finished { get; set; }

        private readonly IPromise<TIn> _parent;
        private readonly Func<TIn, TOut> _mapper;

        private TOut? _cache;

        public MapPromise(IPromise<TIn> parent, Func<TIn, TOut> mapper)
        {
            _parent = parent;
            _mapper = mapper;

            _parent.Canceled += HandleCancel;
            _parent.Finished += HandleFinished;
        }

        public TOut Get()
        {
            _cache ??= _mapper(_parent.Get());
            return _cache;
        }

        public bool HasValue()
        {
            return _parent.HasValue();
        }

        private void HandleCancel(object? sender, EventArgs e)
        {
            Canceled?.Invoke(this, e);
        }

        private void HandleFinished(object? sender, EventArgs e)
        {
            Finished?.Invoke(this, e);
        }
    }
}
