using Cardamom.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Graphics.Ui
{
    [JsonConverter(typeof(ElementSizeConverter))]
    public struct ElementSizeDefinition
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Mode
        {
            Static,
            DynamicContents
        }

        public struct AxisSize
        {
            public Mode Mode { get; set; } = Mode.Static;
            public float Size { get; set; }
            public float MinimumSize { get; set; }
            public float MaximumSize { get; set; } = float.MaxValue;

            public AxisSize() { }

            public override string ToString()
            {
                if (Mode == Mode.Static)
                {
                    return string.Format($"{Mode} {Size}");
                }
                else
                {
                    return string.Format($"{Mode} {MinimumSize}->{MaximumSize}");
                }
            }
        }

        public AxisSize Width { get; set; }
        public AxisSize Height { get; set; }

        public override string ToString()
        {
            return string.Format($"({Width}, {Height})");
        }
    }
}
