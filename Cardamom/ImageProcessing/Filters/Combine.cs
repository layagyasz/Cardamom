using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class Combine : IFilter
    {
        private static readonly Vector2i s_LocalGroupSize = new(32, 32);

        private static ComputeShader? s_CombineShader;
        private static readonly int s_OverflowBehaviorLocation = 0;
        private static readonly int s_LeftTransformLocation = 1;
        private static readonly int s_RightTransformLocation = 2;
        private static readonly int s_BiasLocation = 3;
        private static readonly int s_ChannelLocation = 4;

        private readonly OverflowBehavior _overflowBehavior;
        private readonly Matrix4 _leftTransform;
        private readonly Matrix4 _rightTransform;
        private readonly Vector4 _bias;

        public Combine(OverflowBehavior overflowBehavior, Matrix4 leftTransform, Matrix4 rightTransform, Vector4 bias)
        {
            _overflowBehavior = overflowBehavior;
            _leftTransform = leftTransform;
            _rightTransform = rightTransform;
            _bias = bias;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 2);

            s_CombineShader ??= 
                ComputeShader.FromFile("Resources/ImageProcessing/Filters/combine.comp", s_LocalGroupSize);

            s_CombineShader.SetInt32(s_OverflowBehaviorLocation, (int)_overflowBehavior);
            s_CombineShader.SetMatrix4(s_LeftTransformLocation, _leftTransform);
            s_CombineShader.SetMatrix4(s_RightTransformLocation, _rightTransform);
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
            private OverflowBehavior _overflowBehavior = OverflowBehavior.None;
            private Matrix4 _leftTransform = Matrix4.Identity * 0.5f;
            private Matrix4 _rightTransform = Matrix4.Identity * 0.5f;
            private Vector4 _bias = new();

            public Builder SetOverflowBehavior(OverflowBehavior overflowBehavior)
            {
                _overflowBehavior = overflowBehavior;
                return this;
            }

            public Builder SetLeftTransform(Matrix4 leftTransform)
            {
                _leftTransform = leftTransform;
                return this;
            }

            public Builder SetRightTransform(Matrix4 rightTransform)
            {
                _rightTransform = rightTransform;
                return this;
            }

            public Builder SetBias(Vector4 bias)
            {
                _bias = bias;
                return this;
            }

            public IFilter Build()
            {
                return new Combine(_overflowBehavior, _leftTransform, _rightTransform, _bias);
            }
        }
    }
}
