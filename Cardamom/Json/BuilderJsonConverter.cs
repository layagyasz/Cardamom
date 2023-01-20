using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class BuilderJsonConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.GetCustomAttributes(false).Any(x => x is BuilderClassAttribute);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var attribute = 
                (BuilderClassAttribute)typeToConvert.GetCustomAttributes(false).First(x => x is BuilderClassAttribute);
            return (JsonConverter?)Activator.CreateInstance(
                typeof(BuilderJsonConverterImpl<>).MakeGenericType(typeToConvert), attribute.BuilderType);
        }

        class BuilderJsonConverterImpl<T> : JsonConverter<T>
        {
            private readonly static object[] s_Args = Array.Empty<object>();

            private readonly Type _builderType;
            private readonly MethodInfo _buildMethod;

            public BuilderJsonConverterImpl(Type builderType)
            {
                _builderType = builderType;
                _buildMethod = _builderType.GetMethod("Build")!;
            }

            public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var builder = JsonSerializer.Deserialize(ref reader, _builderType, options);
                return (T?)_buildMethod.Invoke(builder, s_Args);
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }
    }
}
