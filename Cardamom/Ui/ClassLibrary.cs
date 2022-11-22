using Cardamom.Json;
using System.Text.Json;

namespace Cardamom.Ui
{
    public class ClassLibrary
    {
        private readonly Dictionary<string, Class> _classes = new();

        public ClassLibrary(IEnumerable<Class> classes)
        {
            _classes = classes.ToDictionary(x => x.Key, x => x);
        }

        public Class Get(string key)
        {
            return _classes[key];
        }

        public class Builder
        {
            // private readonly Dictionary<string, KeyedWrapper<Font>> _fonts = new();
            private readonly Dictionary<string, Class.Builder> _classes = new();
            
            public Builder ReadFonts(string path)
            {
                /*
                JsonSerializerOptions options = new();
                options.Converters.Add(new FontJsonConverter());
                foreach (var font in 
                    JsonSerializer.Deserialize<List<KeyedWrapper<Font>>>(File.ReadAllText(path), options)!)
                {
                    _fonts.Add(font.Key, font);
                }
                */
                return this;
            }

            public Builder ReadClasses(string directory, string pattern)
            {
                var objects = new Dictionary<string, IKeyed>();
                /*
                foreach (var font in _fonts)
                {
                    objects.Add(font.Key, font.Value);
                }
                */
                foreach (var @class in _classes)
                {
                    objects.Add(@class.Key, @class.Value);
                }
                JsonSerializerOptions options = new();
                options.ReferenceHandler = new KeyedReferenceHandler(objects);
                options.Converters.Add(new ColorJsonConverter());
                options.Converters.Add(new Vector2JsonConverter());
                foreach (var file in Directory.EnumerateFiles(directory, pattern, SearchOption.AllDirectories))
                {
                    foreach (var @class in 
                        JsonSerializer.Deserialize<List<Class.Builder>>(File.ReadAllText(file), options)!)
                    {
                        _classes.Add(@class.Key, @class);
                    }
                }
                return this;
            }

            public ClassLibrary Build()
            {
                return new ClassLibrary(_classes.Values.Select(x => x.Build()));
            }
        }
    }
}
