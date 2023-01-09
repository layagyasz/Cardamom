using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class Adjust : IFilter
    {
        private static ComputeShader? s_AdjustShader;
        private static readonly int s_OverflowBehaviorLocation = 0;
        private static readonly int s_GradientLocation = 1;
        private static readonly int s_BiasLocation = 2;
        private static readonly int s_ChannelLocation = 3;

        private readonly OverflowBehavior _overflowBehavior;
        private readonly Matrix4 _gradient;
        private readonly Vector4 _bias;

        public Adjust(OverflowBehavior overflowBehavior, Matrix4 gradient, Vector4 bias)
        {
            _gradient = gradient;
            _overflowBehavior = overflowBehavior;
            _bias = bias;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            s_AdjustShader ??= ComputeShader.FromFile("Resources/ImageProcessing/Filters/adjust.comp");

            s_AdjustShader.SetInt32(s_OverflowBehaviorLocation, (int)_overflowBehavior);
            s_AdjustShader.SetMatrix4(s_GradientLocation, _gradient);
            s_AdjustShader.SetVector4(s_BiasLocation, _bias);
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
            private OverflowBehavior _overflowBehavior = OverflowBehavior.None;
            private Matrix4 _gradient = Matrix4.Identity;
            private Vector4 _bias = new();

            public Builder SetOverflowBehavior(OverflowBehavior overflowBehavior)
            {
                _overflowBehavior = overflowBehavior;
                return this;
            }

            public Builder SetGradient(Matrix4 gradient)
            {
                _gradient = gradient;
                return this;
            }

            public Builder SetBias(Vector4 bias)
            {
                _bias = bias;
                return this;
            }

            public IFilter Build()
            {
                return new Adjust(_overflowBehavior, _gradient, _bias);
            }
        }
    }
}
