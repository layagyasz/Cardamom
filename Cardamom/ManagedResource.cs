namespace Cardamom
{
    public abstract class ManagedResource : IDisposable
    {
        public void Dispose()
        {
            DisposeImpl();
            GC.SuppressFinalize(this);
            GC.KeepAlive(this);
        }

        protected abstract void DisposeImpl();
    }
}
