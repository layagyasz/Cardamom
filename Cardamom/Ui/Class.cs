namespace Cardamom.Ui
{
    public class Class : IKeyed
    {
        [Flags]
        public enum State
        {
            NONE = 0,
            DISABLE = 1,
            HOVER = 2,
            FOCUS = 4,
            TOGGLE = 8,
        }

        public string Key { get; }

        private ClassAttributes[] _classForStates;

        public Class(string key, ClassAttributes[] classForStates)
        {
            Key = key;
            _classForStates = Precondition.HasSize(classForStates, 16);
        }

        public ClassAttributes Get(State state)
        {
            return _classForStates[(int)state];
        }

        public class Builder : IKeyed
        {
            public class ClassAttributesBuilderWithState {
                public State State { get; set; }
                public ClassAttributes.Builder Attributes { get; set; } = new();
            }

            public string Key { get; set; }
            public Builder? Parent { get; set; }
            public ClassAttributes.Builder? Default { get; set; }
            public List<ClassAttributesBuilderWithState> States { get; set; } = new();

            public Class Build()
            {
                var attributesForStates = new ClassAttributes[16];
                for (int i = 0; i < attributesForStates.Length; ++i)
                {
                    var builder = States.Where(x => (int)x.State == i).FirstOrDefault() ?? new();
                    Precondition.IsNull(attributesForStates[(int)builder.State]);

                    var ancestors = new List<ClassAttributes.Builder>();
                    if (Parent != null)
                    {
                        if (Parent.Default != null)
                        {
                            ancestors.Add(Parent.Default);
                        }
                        ancestors.AddRange(GetAncestry(builder, Parent.States));
                    }
                    if (Default != null)
                    {
                        ancestors.Add(Default);
                    }
                    ancestors.AddRange(GetAncestry(builder, States));
                    attributesForStates[(int)builder.State] = builder.Attributes.Build(ancestors);
                }
                return new Class(Key, attributesForStates);
            }

            private IEnumerable<ClassAttributes.Builder> GetAncestry(
                ClassAttributesBuilderWithState child, IEnumerable<ClassAttributesBuilderWithState> potentialAncestors)
            {
                foreach (var potentialAncestor in potentialAncestors)
                {
                    if (IsAncestor(child, potentialAncestor))
                    {
                        yield return potentialAncestor.Attributes;
                    }
                }
            }

            private bool IsAncestor(ClassAttributesBuilderWithState child, ClassAttributesBuilderWithState ancestor)
            {
                foreach (var state in Enum.GetValues(typeof(State)).Cast<State>())
                {
                    if (child.State.HasFlag(state) || !ancestor.State.HasFlag(state))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
