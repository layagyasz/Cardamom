﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class KeyedJsonConverter<T> : JsonConverter<T> where T : IKeyed
    {
        private readonly Dictionary<string, IKeyed> _objects;

        public KeyedJsonConverter(Dictionary<string, IKeyed> objects)
        {
            _objects = objects;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string stringValue = Precondition.NotNull(reader.GetString());
            return (T)_objects[stringValue];
        }

        public override void Write(Utf8JsonWriter writer, T @object, JsonSerializerOptions options)
        {
            writer.WriteStringValue(@object.Key);
        }
    }
}
