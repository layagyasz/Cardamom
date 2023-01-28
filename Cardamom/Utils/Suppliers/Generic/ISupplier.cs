using Cardamom.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Utils.Suppliers.Generic
{
    [JsonConverter(typeof(GenericJsonConverter))]
    public interface ISupplier
    {
        T Get<T>();
    }
}
