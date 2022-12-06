using Cardamom.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Graphics.Ui
{
    public class Class : IKeyed
    {
        [Flags]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum State
        {
            NONE = 0,
            DISABLE = 1,
            HOVER = 2,
            FOCUS = 4,
            TOGGLE = 8,
        }

        public string Key { get; set; }

        private readonly ClassAttributes[] _attributes;

        public Class(string key, ClassAttributes[] classForStates)
        {
            Key = key;
            _attributes = Precondition.HasSize<ClassAttributes[], ClassAttributes>(classForStates, 16);
        }

        public ClassAttributes Get(State state)
        {
            return _attributes[(int)state];
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
                return new Class(Precondition.IsNotEmpty<string, char>(Key), attributesForStates);
            }

            private static IEnumerable<ClassAttributes.Builder> GetAncestry(
                State state, IEnumerable<ClassAttributesBuilderWithState> potentialAncestors)
            {
                foreach (var potentialAncestor in potentialAncestors)
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
                    || (ancestor < child && (ancestor == State.HOVER || ancestor == State.FOCUS));
            }
        }
    }
}
