using System.Text.Json.Serialization;

namespace Cardamom.ImageProcessing
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Flags]
    public enum Channel
    {
        None = 0,
        Red = 1,
        Green = 2,
        Blue = 4,
        Color = 7,
        Alpha = 8,
        All = 15
    }
}
