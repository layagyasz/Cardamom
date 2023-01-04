using Cardamom.Graphics.Ui;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class ElementSizeConverter : JsonConverter<ElementSizeDefinition>
    {
        class AxisSizeConverter : JsonConverter<ElementSizeDefinition.AxisSize>
        {
            public override ElementSizeDefinition.AxisSize Read(
                ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                ElementSizeDefinition.AxisSize axisSize = new();
                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    if (reader.TokenType == JsonTokenType.String)
                    {
                        axisSize.Mode = JsonSerializer.Deserialize<ElementSizeDefinition.Mode>(ref reader, options);
                    }
                    else if (reader.TokenType == JsonTokenType.Number)
                    {
                        axisSize.Size = reader.GetSingle();
                    }
                }
                else
                {
                    reader.Read();
                    if (reader.TokenType == JsonTokenType.String)
                    {
                        axisSize.Mode = JsonSerializer.Deserialize<ElementSizeDefinition.Mode>(ref reader, options);
                        reader.Read();
                    }

                    List<float> values = new();
                    while (reader.TokenType != JsonTokenType.EndArray)
                    {
                        values.Add(reader.GetSingle());
                        reader.Read();
                    }
                    if (axisSize.Mode == ElementSizeDefinition.Mode.Static)
                    {
                        Precondition.Check(values.Count == 1);
                        axisSize.Size = values[0];
                    }
                    else if (axisSize.Mode == ElementSizeDefinition.Mode.DynamicContents)
                    {
                        Precondition.Check(values.Count <= 2);
                        if (values.Count > 0)
                        {
                            axisSize.MinimumSize = values[0];
                        }
                        if (values.Count > 1)
                        {
                            axisSize.MaximumSize = values[1];
                        }
                    }
                }
                return axisSize;
            }

            public override void Write(
                Utf8JsonWriter writer, ElementSizeDefinition.AxisSize @object, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        public override ElementSizeDefinition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            reader.Read();

            var value = new ElementSizeDefinition();
            var converter = new AxisSizeConverter();
            value.Width = converter.Read(ref reader, typeToConvert, options);
            reader.Read();
            value.Height = converter.Read(ref reader, typeToConvert, options);
            reader.Read();

            if (reader.TokenType != JsonTokenType.EndArray)
            {
                throw new JsonException();
            }
            return value;
        }

        public override void Write(Utf8JsonWriter writer, ElementSizeDefinition @object, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            JsonSerializer.Serialize(writer, @object.Width);
            JsonSerializer.Serialize(writer, @object.Height);
            writer.WriteEndArray();
        }
    }
}
