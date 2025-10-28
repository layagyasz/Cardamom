namespace Cardamom.Graphics.Core
{
    public abstract class GLObject : ManagedResource
    {
        public int Handle { get; }

        protected GLObject(int handle)
        {
            Handle = handle;
        }

        public override bool Equals(object? @object)
        {
            return @object != null
                && @object.GetType() == GetType()
                && @object is GLObject other
                && other.Handle == Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
