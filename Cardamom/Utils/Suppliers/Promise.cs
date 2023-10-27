namespace Cardamom.Utils.Suppliers
{
    public class Promise<T> : ISupplier<T>
    {
        public EventHandler<EventArgs>? Canceled { get; set; }
        public EventHandler<EventArgs>? Finished { get; set; }

        private T? _value;
        private bool _run;

        public void Cancel()
        {
            Monitor.Enter(this);
            Monitor.Pulse(this);
            Monitor.Exit(this);
            Canceled?.Invoke(this, EventArgs.Empty);
        }

        public T Get()
        {
            try
            {
                Monitor.Enter(this);
                if (_run)
                {
                    return _value!;
                }
                else
                {
                    Monitor.Wait(this);
                    return _value!;
                }

            }
            finally
            {
                Monitor.Exit(this);
            }
        }

        public bool HasValue()
        {
            return _run;
        }

        public void Set(T value)
        {
            Monitor.Enter(this);
            _value = value;
            _run = true;
            Monitor.Pulse(this);
            Monitor.Exit(this);
            Finished?.Invoke(this, EventArgs.Empty);
        }
    }
}
