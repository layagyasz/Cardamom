using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class Combine : IFilter
    {
        private static ComputeShader? s_CombineShader;
        private static readonly int s_OverflowBehaviorLocation = 0;
        private static readonly int s_LeftFactorLocation = 1;
        private static readonly int s_RightFactorLocation = 2;
        private static readonly int s_BiasLocation = 3;
        private static readonly int s_ChannelLocation = 4;

        private readonly OverflowBehavior _overflowBehavior;
        private readonly Vector4 _leftFactor;
        private readonly Vector4 _rightFactor;
        private readonly Vector4 _bias;

        public Combine(OverflowBehavior overflowBehavior, Vector4 leftFactor, Vector4 rightFactor, Vector4 bias)
        {
            _overflowBehavior = overflowBehavior;
            _leftFactor = leftFactor;
            _rightFactor = rightFactor;
            _bias = bias;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 2);

            s_CombineShader ??= ComputeShader.FromFile("Resources/ImageProcessing/Filters/combine.comp");

            s_CombineShader.SetInt32(s_OverflowBehaviorLocation, (int)_overflowBehavior);
            s_CombineShader.SetVector4(s_LeftFactorLocation, _leftFactor);
            s_CombineShader.SetVector4(s_RightFactorLocation, _rightFactor);
            s_CombineShader.SetVector4(s_BiasLocation, _bias);
            s_CombineShader.SetInt32(s_ChannelLocation, (int)channel);

            var leftTex = inputs["left"].GetTexture();
            var rightTex = inputs["right"].GetTexture();
            var outTex = output.GetTexture();
            leftTex.BindImage(0);
            rightTex.BindImage(1);
            outTex.BindImage(2);
            s_CombineShader.DoCompute(leftTex.Size);
            Texture.UnbindImage(0);
            Texture.UnbindImage(1);
            Texture.UnbindImage(2);
        }

        public class Builder : IFilter.IFilterBuilder
        {
            private OverflowBehavior _overflowBehavior = OverflowBehavior.Clamp;
            private Vector4 _leftFactor = new(0.5f, 0.5f, 0.5f, 0.5f);
            private Vector4 _rightFactor = new(0.5f, 0.5f, 0.5f, 0.5f);
            private Vector4 _bias = new();

            public Builder SetOverflowBehavior(OverflowBehavior overflowBehavior)
            {
                _overflowBehavior = overflowBehavior;
                return this;
            }

            public Builder SetLeftFactor(Vector4 leftFactor)
            {
                _leftFactor = leftFactor;
                return this;
            }

            public Builder SetRightFactor(Vector4 rightFactor)
            {
                _rightFactor = rightFactor;
                return this;
            }

            public Builder SetBias(Vector4 bias)
            {
                _bias = bias;
                return this;
            }

            public IFilter Build()
            {
                return new Combine(_overflowBehavior, _leftFactor, _rightFactor, _bias);
            }
        }
    }
}
