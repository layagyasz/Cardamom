namespace Cardamom.Graphics
{
    public abstract class GraphicsResource : IDisposable
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
