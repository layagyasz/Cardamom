using Cardamom.Graphics;
using OpenTK.Mathematics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json.Graphics
{
    public class ShaderJsonConverter : JsonConverter<RenderShader>
    {
        public override RenderShader Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var builder = JsonSerializer.Deserialize<RenderShader.Builder>(ref reader, options)!;
            return builder.Build();
        }

        public override void Write(Utf8JsonWriter writer, RenderShader @object, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
