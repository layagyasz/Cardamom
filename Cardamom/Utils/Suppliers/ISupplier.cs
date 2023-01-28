using Cardamom.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Utils.Suppliers
{
    [JsonConverter(typeof(GenericJsonConverter))]
    public interface ISupplier<T> : Generic.ISupplier
    {
        T Get();

        TOut Generic.ISupplier.Get<TOut>()
        {
            return (TOut)(object)Get()!;
        }
    }
}
