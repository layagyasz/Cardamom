using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class Adjust : IFilter
    {
        private static Shader? s_AdjustShader;
        private static readonly int s_OverflowBehaviorLocation = 0;
        private static readonly int s_AdjustmentLocation = 1;
        private static readonly int s_ChannelLocation = 2;

        public enum OverflowBehavior
        {
            NONE = 0,
            CLAMP = 1,
            MODULUS = 2
        }

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

            s_AdjustShader ??= new Shader.Builder().SetCompute("Resources/adjust.comp").Build();

            s_AdjustShader.SetInt32(s_OverflowBehaviorLocation, (int)_overflowBehavior);
            s_AdjustShader.SetVector4(s_AdjustmentLocation, _adjustment);

            s_AdjustShader.SetInt32(s_ChannelLocation, (int)channel);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            s_AdjustShader.DoCompute(inTex.Size);
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
