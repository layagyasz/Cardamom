using System.Text.Json.Serialization;

namespace Cardamom.Ui
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum VerticalAlignment
    {
        Top,
        Center,
        Bottom
    }
}
