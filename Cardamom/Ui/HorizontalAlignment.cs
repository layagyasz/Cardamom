using System.Text.Json.Serialization;

namespace Cardamom.Ui
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right
    }
}
