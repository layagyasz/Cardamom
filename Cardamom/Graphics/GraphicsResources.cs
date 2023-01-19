using Cardamom.Collections;
using Cardamom.Graphics.TexturePacking;
using Cardamom.Graphics.Ui;
using Cardamom.Json;
using Cardamom.Json.Collections;
using Cardamom.Json.Graphics;
using Cardamom.Json.Graphics.TexturePacking;
using Cardamom.Json.OpenTK;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Graphics
{
    [JsonConverter(typeof(GraphicsResourcesJsonConverter))]
    public class GraphicsResources
    {
        private readonly TextureLibrary _textures;
        private readonly Library<RenderShader> _shaders;
        private readonly Library<Class> _classes = new();

        public GraphicsResources(
            TextureLibrary textures, Library<RenderShader> shaders, Library<Class> classes)
        {
            _textures = textures;
            _shaders = shaders;
            _classes = classes;
        }

        public Class GetClass(string key)
        {
            return _classes[key];
        }

        public RenderShader GetShader(string key)
        {
            return _shaders[key];
        }

        public class Builder
        {
            [JsonConverter(typeof(FromFileJsonConverter))]
            public TextureLibrary Textures { get; set; } = TextureLibrary.Empty;

            [JsonConverter(typeof(FromFileJsonConverter))]
            public Library<KeyedWrapper<Font>> Fonts { get; set; } = new();

            [JsonConverter(typeof(FromFileJsonConverter))]
            public Library<KeyedWrapper<RenderShader>> Shaders { get; set; } = new();

            [JsonConverter(typeof(FromMultipleFileJsonConverter))]
            public Library<Class.Builder> Classes { get; set; } = new();

            public static Builder ReadFrom(string path)
            {
                JsonSerializerOptions options = new()
                {
                    ReferenceHandler = new KeyedReferenceHandler(new Dictionary<string, IKeyed>())
                };
                options.Converters.Add(new ColorJsonConverter());
                options.Converters.Add(new Vector2JsonConverter());
                options.Converters.Add(new Vector2iJsonConverter());
                options.Converters.Add(new FontJsonConverter());
                options.Converters.Add(new ShaderJsonConverter());
                options.Converters.Add(new TextureLibraryJsonConverter());
                options.Converters.Add(new LibraryJsonConverter());
                return JsonSerializer.Deserialize<Builder>(File.ReadAllText(path), options)!;
            }

            public GraphicsResources Build()
            {
                return new GraphicsResources(
                    Textures, 
                    Shaders.ToLibrary(x => x.Key, x => x.Value.Element!),
                    Classes.ToLibrary(x => x.Key, x => x.Value.Build()));
            }
        }
    }
}
