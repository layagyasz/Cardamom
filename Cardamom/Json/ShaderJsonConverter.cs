using Cardamom.Graphics;
using OpenTK.Mathematics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class ShaderJsonConverter : JsonConverter<Shader>
    {
        public override Shader Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var builder = JsonSerializer.Deserialize<Shader.Builder>(ref reader, options)!;
            return builder.Build();
        }

        public override void Write(Utf8JsonWriter writer, Shader @object, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
