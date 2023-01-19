using Cardamom.Json.Collections;
using System.Text.Json.Serialization;

namespace Cardamom.Collections
{
    [JsonConverter(typeof(LibraryJsonConverter))]
    public class Library<T> : Dictionary<string, T> { }
}
