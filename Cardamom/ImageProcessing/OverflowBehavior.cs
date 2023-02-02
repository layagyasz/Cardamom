using System.Text.Json.Serialization;

namespace Cardamom.ImageProcessing
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OverflowBehavior
    {
        None = 0,
        Clamp = 1,
        Modulus = 2
    }
}
