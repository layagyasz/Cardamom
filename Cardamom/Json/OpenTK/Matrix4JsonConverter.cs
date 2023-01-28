using OpenTK.Mathematics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json.OpenTK
{
    public class Matrix4JsonConverter : JsonConverter<Matrix4>
    {
        public override Matrix4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var rows = JsonSerializer.Deserialize<Vector4[]>(ref reader, options)!;
            if (rows.Length != 4)
            {
                throw new JsonException();
            }
            return new(rows[0], rows[1], rows[2], rows[3]);
        }

        public override void Write(Utf8JsonWriter writer, Matrix4 @object, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize<Vector4[]>(
                writer, new Vector4[] { @object.Row0, @object.Row1, @object.Row2, @object.Row3 }, options);
        }
    }
}