using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class KeyedReferenceHandler : ReferenceHandler
    {
        private readonly KeyedReferenceResolver _resolver;

        public KeyedReferenceHandler()
            : this(new(), new()) { }

        public KeyedReferenceHandler(
            Dictionary<string, IKeyed> keyedObjects, Dictionary<string, object> unkeyedObjects)
        {
            _resolver = new(keyedObjects, unkeyedObjects);
        }

        public override ReferenceResolver CreateResolver()
        {
            return _resolver;
        }
    }
}
