using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    public class Adjust : IFilter
    {
        private static Shader? ADJUST_SHADER;
        private static readonly int OVERFLOW_BEHAVIOR_LOCATION = 0;
        private static readonly int ADJUSTMENT_LOCATION = 1;
        private static readonly int CHANNEL_LOCATION = 2;

        public enum OverflowBehavior
        {
            NONE = 0,
            CLAMP = 1,
            MODULUS = 2
        }

        public bool InPlace => true;

        private readonly OverflowBehavior _overflowBehavior;
        private readonly Vector4 _adjustment;

        public Adjust(OverflowBehavior overflowBehavior, Vector4 adjustment)
        {
            _overflowBehavior = overflowBehavior;
            _adjustment = adjustment;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            ADJUST_SHADER ??= new Shader.Builder().SetCompute("Resources/adjust.comp").Build();

            ADJUST_SHADER.SetInt32(OVERFLOW_BEHAVIOR_LOCATION, (int)_overflowBehavior);
            ADJUST_SHADER.SetVector4(ADJUSTMENT_LOCATION, _adjustment);

            ADJUST_SHADER.SetInt32(CHANNEL_LOCATION, (int)channel);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            ADJUST_SHADER.DoCompute(inTex.Size);
            Texture.UnbindImage(0);
            Texture.UnbindImage(1);
        }

        public class Builder : IFilter.IFilterBuilder
        {
            private OverflowBehavior _overflowBehavior = OverflowBehavior.CLAMP;
            private Vector4 _adjustment = new(0, 0, 0, 0);

            public Builder SetOverflowBehavior(OverflowBehavior overflowBehavior)
            {
                _overflowBehavior = overflowBehavior;
                return this;
            }

            public Builder SetAdjustment(Vector4 adjustment)
            {
                _adjustment = adjustment;
                return this;
            }

            public IFilter Build()
            {
                return new Adjust(_overflowBehavior, _adjustment);
            }
        }
    }
}
