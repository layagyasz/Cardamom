using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class KeyedReferenceResolver : ReferenceResolver
    {
        private readonly Dictionary<string, IKeyed> _objects;

        public KeyedReferenceResolver(Dictionary<string, IKeyed> objects)
        {
            _objects = objects;
        }

        public override object ResolveReference(string referenceId)
        {
            return _objects[referenceId];
        }

        public override string GetReference(object value, out bool alreadyExists)
        {
            if (value is IKeyed keyed)
            {
                alreadyExists = _objects.ContainsKey(keyed.Key);
                return keyed.Key;
            }
            throw new JsonException("Type must be derived from IKeyed.");
        }

        public override void AddReference(string referenceId, object value)
        {
            if (value is IKeyed keyed)
            {
                if (keyed.Key != string.Empty && keyed.Key != referenceId)
                {
                    throw new JsonException("Key and $id have conflicting values.");
                }
                keyed.Key = referenceId;
                _objects.Add(referenceId, keyed);
            }
            else
            {
                throw new JsonException("Type must be derived from IKeyed.");
            }
        }
    }
}
