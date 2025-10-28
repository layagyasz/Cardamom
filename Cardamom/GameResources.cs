using Cardamom.Audio;
using Cardamom.Collections;
using Cardamom.Graphics;
using Cardamom.Graphics.TexturePacking;
using Cardamom.Json;
using Cardamom.Json.Audio;
using Cardamom.Json.Collections;
using Cardamom.Json.OpenTK;
using Cardamom.Ui;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom
{
    [JsonConverter(typeof(BuilderJsonConverter))]
    [BuilderClass(typeof(Builder))]
    public class GameResources : ManagedResource
    {
        private readonly Library<ISound> _sounds;
        private TextureLibrary? _textures;
        private Library<RenderShader>? _shaders;
        private Library<Class>? _classes = new();

        public GameResources(
            Library<ISound> sounds, TextureLibrary textures, Library<RenderShader> shaders, Library<Class> classes)
        {
            _sounds = sounds;
            _textures = textures;
            _shaders = shaders;
            _classes = classes;
        }

        protected override void DisposeImpl()
        {
            _textures!.Dispose();
            _textures = null;
            foreach (var shader in _shaders!.Values)
            {
                shader.Dispose();
            }
            _shaders = null;
            foreach (var @class in _classes!.Values)
            {
                @class.Dispose();
            }
            _classes = null;
        }

        public Class GetClass(string key)
        {
            return _classes![key];
        }

        public RenderShader GetShader(string key)
        {
            return _shaders![key];
        }

        public ISound GetSound(string key)
        {
            return _sounds[key];
        }

        public TextureSegment GetTexture(string key)
        {
            return _textures!.Get(key);
        }

        public class Builder
        {
            [JsonConverter(typeof(FromFileJsonConverter))]
            public Library<KeyedWrapper<ISound>> Sounds { get; set; } = new();
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
                    ReferenceHandler = new KeyedReferenceHandler()
                };
                options.Converters.Add(new ColorJsonConverter());
                options.Converters.Add(new Vector2JsonConverter());
                options.Converters.Add(new Vector2iJsonConverter());
                options.Converters.Add(new FileStreamSoundConverter());
                return JsonSerializer.Deserialize<Builder>(File.ReadAllText(path), options)!;
            }

            public GameResources Build()
            {
                var resources = new Class.BuilderResources();
                return new GameResources(
                    Sounds.ToLibrary(x => x.Key, x => x.Value.Element!),
                    Textures,
                    Shaders.ToLibrary(x => x.Key, x => x.Value.Element!),
                    Classes.ToLibrary(x => x.Key, x => x.Value.Build(resources)));
            }
        }
    }
}
