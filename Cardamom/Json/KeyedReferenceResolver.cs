using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class KeyedReferenceResolver : ReferenceResolver
    {
        private readonly Dictionary<string, IKeyed> _keyedObjects;
        private readonly Dictionary<string, object> _unkeyedObjects;

        public KeyedReferenceResolver(Dictionary<string, IKeyed> objects, Dictionary<string, object> unkeyedObjects)
        {
            _keyedObjects = objects;
            _unkeyedObjects = unkeyedObjects;
        }

        public override object ResolveReference(string referenceId)
        {
            if (_keyedObjects.TryGetValue(referenceId, out var value))
            {
                return value;
            }
            return _unkeyedObjects[referenceId];
        }

        public override string GetReference(object value, out bool alreadyExists)
        {
            if (value is IKeyed keyed)
            {
                alreadyExists = _keyedObjects.ContainsKey(keyed.Key!);
                return keyed.Key;
            }
            else
            {
                alreadyExists = _unkeyedObjects.ContainsValue(value);
                if (alreadyExists)
                {
                    return _unkeyedObjects.First(x => Equals(x.Value, value)).Key;
                }
                return string.Format($"unkeyed-{_unkeyedObjects.Count}");
            }
        }

        public override void AddReference(string referenceId, object value)
        {
            if (value is IKeyed keyed)
            {
                if (keyed.Key != null && keyed.Key != string.Empty && !Equals(keyed.Key, referenceId))
                {
                    throw new JsonException("Key and $id have conflicting values.");
                }
                keyed.Key = referenceId;
                _keyedObjects.Add(referenceId, keyed);
            }
            else
            {
                _unkeyedObjects.Add(referenceId, value);
            }
        }
    }
}
