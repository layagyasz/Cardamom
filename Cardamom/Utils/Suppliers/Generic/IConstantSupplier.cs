using Cardamom.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Utils.Suppliers.Generic
{
    [JsonConverter(typeof(GenericJsonConverter))]
    public interface IConstantSupplier : ISupplier
    {
        void Set(object value);
    }
}
