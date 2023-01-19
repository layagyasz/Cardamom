using Cardamom.Graphics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json.Graphics
{
    public class GraphicsResourcesJsonConverter : JsonConverter<GraphicsResources>
    {
        public override GraphicsResources Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var builder = JsonSerializer.Deserialize<GraphicsResources.Builder>(ref reader, options)!;
            return builder.Build();
        }

        public override void Write(Utf8JsonWriter writer, GraphicsResources @object, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
