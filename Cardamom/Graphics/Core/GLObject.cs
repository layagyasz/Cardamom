namespace Cardamom.Graphics.Core
{
    public abstract class GLObject : IDisposable
    {
        public int Handle { get; }

        protected GLObject(int handle)
        {
            Handle = handle;
        }

        public override bool Equals(object? @object)
        {
            return @object != null
                && @object is GLObject other
                && other.Handle == Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }

        public void Dispose()
        {
            DisposeImpl();
            GC.SuppressFinalize(this);
            GC.KeepAlive(this);
        }

        protected abstract void DisposeImpl();
    }
}
