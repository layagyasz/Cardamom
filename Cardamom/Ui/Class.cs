using Cardamom.Graphics;
using Cardamom.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Ui
{
    public class Class : GraphicsResource, IKeyed
    {
        [Flags]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum State
        {
            None = 0,
            Disable = 1,
            Hover = 2,
            Focus = 4,
            Toggle = 8,
        }

        public string Key { get; set; }

        private ClassAttributes[]? _attributes;

        public Class(string key, ClassAttributes[] classForStates)
        {
            Key = key;
            _attributes = Precondition.HasSize<ClassAttributes[], ClassAttributes>(classForStates, 16);
        }

        protected override void DisposeImpl()
        {
            foreach (var attributes in _attributes!)
            {
                attributes.Dispose();
            }
            _attributes = null;
        }

        public ClassAttributes Get(State state)
        {
            return _attributes![(int)state];
        }

        public class Builder : IKeyed
        {
            public class ClassAttributesBuilderWithState
            {
                [JsonConverter(typeof(FlagJsonConverter<State>))]
                public State State { get; set; }
                public ClassAttributes.Builder Attributes { get; set; } = new();
            }

            public string Key { get; set; } = string.Empty;
            [JsonConverter(typeof(ReferenceJsonConverter))]
            public Builder? Parent { get; set; }
            public ClassAttributes.Builder? Default { get; set; }
            public List<ClassAttributesBuilderWithState> States { get; set; } = new();

            public Class Build()
            {
                var attributesForStates = new ClassAttributes[16];
                for (int i = 0; i < attributesForStates.Length; ++i)
                {
                    var builder = States.Where(x => (int)x.State == i).FirstOrDefault() ?? new();
                    Precondition.IsNull(attributesForStates[i]);

                    var ancestors = new List<ClassAttributes.Builder>();
                    var p = this;
                    while (p != null)
                    {
                        ancestors.AddRange(GetAncestry((State)i, p.States));
                        if (p.Default != null)
                        {
                            ancestors.Add(p.Default);
                        }
                        p = p.Parent;
                    }
                    attributesForStates[i] = builder.Attributes.Build(ancestors);
                }
                return new Class(Precondition.IsNotEmpty<string, char>(Key!), attributesForStates);
            }

            private static IEnumerable<ClassAttributes.Builder> GetAncestry(
                State state, IEnumerable<ClassAttributesBuilderWithState> potentialAncestors)
            {
                foreach (var potentialAncestor in potentialAncestors.OrderBy(x => -(int)x.State))
                {
                    if (IsAncestor(state, potentialAncestor.State))
                    {
                        yield return potentialAncestor.Attributes;
                    }
                }
            }

            private static bool IsAncestor(State child, State ancestor)
            {
                return (ancestor & ~child) == 0
                    || ancestor < child && (ancestor == State.Hover || ancestor == State.Focus);
            }
        }
    }
}
