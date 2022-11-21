using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class KeyedReferenceHandler : ReferenceHandler
    {
        private readonly KeyedReferenceResolver _resolver;

        public KeyedReferenceHandler(Dictionary<string, IKeyed> objects)
        {
            _resolver = new(objects);
        }

        public override ReferenceResolver CreateResolver()
        {
            return _resolver;
        }
    }
}
