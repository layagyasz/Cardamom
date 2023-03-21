using Cardamom.Graphics;
using Cardamom.Json;
using OpenTK.Mathematics;
using System.Collections;
using System.Text.Json.Serialization;

namespace Cardamom.Ui
{
    public class Class : GraphicsResource, IKeyed
    {
        private static readonly string[] s_Uniforms =
            { "foreground_color", "border_width", "border_color", "corner_radius" };

        public class BuilderResources
        {
            private Dictionary<UniformBufferKey, UniformBuffer> _uniformBuffers = new();

            public UniformBuffer Get(UniformBufferKey key)
            {
                if (_uniformBuffers.TryGetValue(key, out var uniformBuffer))
                {
                    return uniformBuffer;
                }
                uniformBuffer = new UniformBuffer(key.Shader.GetUniformBlockSize("settings"));
                var offsets = key.Shader.GetUniformOffsets(s_Uniforms);
                uniformBuffer.Set(offsets[0], 4 * sizeof(float), key.ForegroundColor);
                uniformBuffer.SetArray(offsets[1], sizeof(float), key.BorderWidth);
                uniformBuffer.SetArray(offsets[2], 4 * sizeof(float), key.BorderColor);
                uniformBuffer.SetArray(offsets[3], 2 * sizeof(float), key.CornerRadius);
                _uniformBuffers.Add(key, uniformBuffer);
                return uniformBuffer;
            }
        }

        public class UniformBufferKey
        {
            public RenderShader Shader { get; }
            public Color4 ForegroundColor;
            public float[] BorderWidth { get; }
            public Color4[] BorderColor { get; }
            public Vector2[] CornerRadius { get; }

            public UniformBufferKey(
                RenderShader shader, 
                Color4 foregroundColor,
                float[] borderWidth, 
                Color4[] borderColor,
                Vector2[] cornerRadius)
            {
                Shader = shader;
                ForegroundColor = foregroundColor;
                BorderWidth = borderWidth;
                BorderColor = borderColor;
                CornerRadius = cornerRadius;
            }

            public override bool Equals(object? @object)
            {
                if (@object is UniformBufferKey other)
                {
                    return ForegroundColor == other.ForegroundColor
                        && BorderWidth.SequenceEqual(other.BorderWidth) 
                        && BorderColor.SequenceEqual(other.BorderColor) 
                        && CornerRadius.SequenceEqual(other.CornerRadius);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(
                    ForegroundColor, HashArray(BorderWidth), HashArray(BorderColor), HashArray(CornerRadius));
            }
            
            private static int HashArray<T>(T[] array)
            {
                return ((IStructuralEquatable)array).GetHashCode(EqualityComparer<T>.Default);
            }
        }

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

            public Class Build(BuilderResources resources)
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
                    attributesForStates[i] = builder.Attributes.Build(resources, ancestors);
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
