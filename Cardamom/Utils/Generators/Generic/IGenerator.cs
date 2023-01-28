using Cardamom.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Utils.Generators.Generic
{
    [JsonConverter(typeof(GenericJsonConverter))]
    public interface IGenerator
    {
        T Generate<T>(Random random);
    }
}
