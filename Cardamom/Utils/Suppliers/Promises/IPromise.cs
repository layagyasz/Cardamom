namespace Cardamom.Utils.Suppliers.Promises
{
    public interface IPromise<T> : ISupplier<T>
    {
        EventHandler<EventArgs>? Canceled { get; set; }
        EventHandler<EventArgs>? Finished { get; set; }

        bool HasValue();
    }
}
